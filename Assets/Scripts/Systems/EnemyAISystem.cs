using Entitas;
using System;
using UnityEngine;

public class EnemyAISystem : IInitializeSystem, IExecuteSystem
{
    private GameContext context;
    private GameEntity enemyGameEntity;
    private GameEntity threat;

    public Vector3[] shelters;
    private Vector3 currentSafePoint;
    private bool safePositionFound = false;

    private float attackRecoverHealthThreshold;
    private float attackDistanceSqr;

    //ToDo: shortcut; not quite entitas way, but ultimately non-determinisic input has to be recorded in some way, anyway
    private Func<Vector3, bool> isPositionSafeCallback;

    public EnemyAISystem(GameContext context)
    {
        this.context = context;
    }

    public void Initialize()
    {
        var enemyGroup = context.GetGroup(GameMatcher.Enemy);
        enemyGameEntity = enemyGroup.GetSingleEntity();
        threat = enemyGameEntity.enemy.target;

        shelters = enemyGameEntity.aIPerception.stationaryPositions;

        attackRecoverHealthThreshold = enemyGameEntity.aIPerception.attackRecoverHealthThreshold;
        attackDistanceSqr = enemyGameEntity.aIPerception.attackDistance * enemyGameEntity.aIPerception.attackDistance;
        isPositionSafeCallback = enemyGameEntity.aIPerception.callback.IsPositionSafe;
    }

    public void Execute()
    {
        if (!enemyGameEntity.isEnabled)
        {
            return;
        }

        if (GetNormalizedHealth() < attackRecoverHealthThreshold)
        {
            SetTrigger(false);
            SeekSafety();
        }
        else
        {
            if (threat.isEnabled)
            {
                Attack();
            }
            else
            {
                SetTrigger(false);
            }
        }
    }

    private float GetNormalizedHealth()
    {
        return (float)enemyGameEntity.health.healthPoints / enemyGameEntity.health.healthPointsCap;
    }

    private void SetTrigger(bool pull)
    {
        if (enemyGameEntity.gun.triggerDown != pull)
        {
            enemyGameEntity.gun.triggerDown = pull;
        }
    }

    private void Attack()
    {
        var distance = threat.position.position - enemyGameEntity.position.position;

        bool desiredDistance = distance.sqrMagnitude > attackDistanceSqr; //sqr is cheaper

        SetTrigger(!desiredDistance);

        if (desiredDistance)
        {
            enemyGameEntity.ReplaceMovementDirection(threat.position.position, newOnlyRotationAffected:false);
        }
        else
        {
            //ToDo: bit hacky, set position to current position in order to stop
            enemyGameEntity.ReplaceMovementDirection(enemyGameEntity.position.position, newOnlyRotationAffected: false);
            //effectively: rotation
            enemyGameEntity.ReplaceMovementDirection(threat.position.position, newOnlyRotationAffected: true);
        }

    }

    private void SeekSafety()
    {
        if (!safePositionFound)
        {
            SetShelterMaximizingReachingChance();

            if (safePositionFound)
            {
                enemyGameEntity.ReplaceMovementDirection(currentSafePoint, newOnlyRotationAffected: false);
            }
        }
        else
        {
            if (!isPositionSafeCallback(currentSafePoint))
            {
                enemyGameEntity.ReplaceMovementDirection(enemyGameEntity.position.position, newOnlyRotationAffected: false);

                safePositionFound = false;
            }
        }
    }

    //maximizing value of shelter choice
    private void SetShelterMaximizingReachingChance()
    {
        float bestSafetyValue = 0;

        foreach (var shelter in shelters)
        {
            if (isPositionSafeCallback(shelter))
            {
                float safetyValue = AssessChanceOfReachingSafely(shelter);

                if (safetyValue > bestSafetyValue)
                {
                    bestSafetyValue = safetyValue;
                    currentSafePoint = shelter;
                    safePositionFound = true;
                }
            }
        }
    }


    //heuristics
    private float AssessChanceOfReachingSafely(Vector3 shelterTransform)
    {
        Vector3 threatDistance = threat.position.position - enemyGameEntity.position.position;
        Vector3 safePointDistance = shelterTransform - enemyGameEntity.position.position;

        return Mathf.Abs(Vector3.Angle(threatDistance, safePointDistance));
    }


}

