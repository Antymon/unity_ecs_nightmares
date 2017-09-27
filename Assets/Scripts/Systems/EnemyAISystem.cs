﻿using Entitas;
using System;
using UnityEngine;

public class EnemyAISystem : IInitializeSystem, IExecuteSystem
{
    private StateController<AIState> stateController;

    //EnemySpawningModel enemySpawningModel = new EnemySpawningModel();

    public enum AIState
    {
        CHOOSE,
        ATTACK,
        HIDE
    };

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
        otherGameEntity = selfGameEntity.agent.target;

        shelterPoints = selfGameEntity.aIPerception.stationaryPositions;

        attackRecoverHealthThreshold = selfGameEntity.aIPerception.attackRecoverHealthThreshold;
        attackDistanceSqr = selfGameEntity.aIPerception.attackDistance * selfGameEntity.aIPerception.attackDistance;
        isPositionSafeCallback = selfGameEntity.aIPerception.callback.IsPositionSafe;

        stateController = new StateController<AIState>(AIState.CHOOSE);

        stateController.AddState(AIState.CHOOSE, Choose);
        stateController.AddState(AIState.ATTACK, Attack);
        stateController.AddState(AIState.HIDE, Hide);

    }

    public void Execute()
    {
        if (!selfGameEntity.isEnabled)
        {
            return;
        }

        stateController.Update();
    }

    void Choose(StateController<AIState>.Stage stage, AIState state)
    {
        if (stage == StateController<AIState>.Stage.ENTER)
        {
            if (GetNormalizedHealth() < attackRecoverHealthThreshold)
            {
                stateController.SetNextState(AIState.HIDE);
            }
            else
            {
                if (otherGameEntity.isEnabled)
                {
                    stateController.SetNextState(AIState.ATTACK);
                }
            }
        }
    }

    void Attack(StateController<AIState>.Stage stage, AIState state)
    {
        if (stage == StateController<AIState>.Stage.ENTER)
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

            stateController.SetNextState(AIState.CHOOSE);
        }
        else if (stage == StateController<AIState>.Stage.LEAVE)
        {
            SetTrigger(false);
        }

    }

    void Hide(StateController<AIState>.Stage stage, AIState state)
    {
        if (stage == StateController<AIState>.Stage.ENTER)
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

            stateController.SetNextState(AIState.CHOOSE);
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

    //heuristic measures
    private float AssessChanceOfReachingSafely(Vector3 shelterTransform)
    {
        Vector3 threatDistance = otherGameEntity.position.position - selfGameEntity.position.position;
        Vector3 safePointDistance = shelterTransform - selfGameEntity.position.position;
        float safePointDistanceMaganitude = safePointDistance.magnitude;

        //favorable safety metrics:

        //the bigger angle with enemy, the better - don't run straight into the fire
        var normalizedAngle = Mathf.Abs(Vector3.Angle(threatDistance, safePointDistance)) / 180f;

        //safe point - the closer, the better
        var distanceInverted = safePointDistanceMaganitude > 1f ? 1 / safePointDistanceMaganitude : 1f;

        return normalizedAngle + distanceInverted;
    }
}