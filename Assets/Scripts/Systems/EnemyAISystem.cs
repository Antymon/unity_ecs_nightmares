using Entitas;
using System;
using UnityEngine;

public class EnemyAISystem : IInitializeSystem, IExecuteSystem
{
    private GameContext context;
    private GameEntity selfGameEntity;
    private GameEntity otherGameEntity;

    public Vector3[] shelterPoints;
    private Vector3 currentSafePoint;
    private bool safePositionFound = false;

    private float attackRecoverHealthThreshold;
    private float attackDistanceSqr;

    //ToDo: shortcut; not quite entitas way, but ultimately non-determinisic input has to be recorded in some way, anyway
    //calling into relevant behaviour
    private Func<Vector3, bool> isPositionSafeCallback;

    public EnemyAISystem(GameContext context)
    {
        this.context = context;
    }

    public void Initialize()
    {
        var enemyGroup = context.GetGroup(GameMatcher.Enemy);
        selfGameEntity = enemyGroup.GetSingleEntity();
        otherGameEntity = selfGameEntity.enemy.target;

        shelterPoints = selfGameEntity.aIPerception.stationaryPositions;

        attackRecoverHealthThreshold = selfGameEntity.aIPerception.attackRecoverHealthThreshold;
        attackDistanceSqr = selfGameEntity.aIPerception.attackDistance * selfGameEntity.aIPerception.attackDistance;
        isPositionSafeCallback = selfGameEntity.aIPerception.callback.IsPositionSafe;
    }

    public void Execute()
    {
        if (!selfGameEntity.isEnabled)
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
            if (otherGameEntity.isEnabled)
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
        return (float)selfGameEntity.health.healthPoints / selfGameEntity.health.healthPointsCap;
    }

    private void SetTrigger(bool pull)
    {
        if (selfGameEntity.gun.triggerDown != pull)
        {
            selfGameEntity.gun.triggerDown = pull;
        }
    }

    private void Attack()
    {
        var distance = otherGameEntity.position.position - selfGameEntity.position.position;

        bool isDesiredDistance = distance.sqrMagnitude <= attackDistanceSqr; //sqr is cheaper

        SetTrigger(isDesiredDistance); //shoot or not shoot

        if (isDesiredDistance) //stop, aim
        {
            selfGameEntity.ReplaceMovementDestination(selfGameEntity.position.position, otherGameEntity.position.position);
        }
        else //chase
        {
            selfGameEntity.ReplaceMovementDestination(otherGameEntity.position.position, otherGameEntity.position.position);
        }

    }

    private void SeekSafety()
    {
        if (!safePositionFound)
        {
            SetShelterMaximizingReachingChance();

            if (safePositionFound)
            {
                //head safe position
                selfGameEntity.ReplaceMovementDestination(currentSafePoint, currentSafePoint);
            }
        }
        else
        {
            if (!isPositionSafeCallback(currentSafePoint))
            {
                //stop
                selfGameEntity.ReplaceMovementDestination(selfGameEntity.position.position, selfGameEntity.position.position);

                safePositionFound = false;
            }
        }
    }

    //maximizing value of shelter choice
    private void SetShelterMaximizingReachingChance()
    {
        float bestSafetyValue = 0;

        foreach (var shelter in shelterPoints)
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
        Vector3 threatDistance = otherGameEntity.position.position - selfGameEntity.position.position;
        Vector3 safePointDistance = shelterTransform - selfGameEntity.position.position;

        return Mathf.Abs(Vector3.Angle(threatDistance, safePointDistance));
    }


}

