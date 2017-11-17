using DG.Tweening;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : IInitializeSystem, IExecuteSystem
{
    private GameContext gameContext;
    private InputContext inputContext;

    private IGroup<GameEntity> agentsGroup;
    private IGroup<GameEntity> roundStartedGroup;
    private IGroup<GameEntity> roundFinishedGroup;
    private IGroup<GameEntity> effectsGroup;
    private IGroup<GameEntity> enemyGroup;

    private IEffectsFactory effectsFactory;

    private List<IEffect> effectsToRemove;
    private List<Tween> spawnDelayedCalls;

    private EntityPrefabNameBinding[] spawnableEffects = new EntityPrefabNameBinding[] 
    {
        EntityPrefabNameBinding.EFFECT_ADD_HEALTH_BINDING,
        EntityPrefabNameBinding.EFFECT_MOVEMENT_INVERTER_BINDING
    };
    

    public EffectSystem(GameContext gameContext,InputContext inputContext, IEntityDeserializer entityDeserializer)
    {
        this.gameContext = gameContext;
        this.inputContext = inputContext;

        effectsFactory = new EffectsFactory(gameContext,entityDeserializer);

        effectsToRemove = new List<IEffect>();
        spawnDelayedCalls = new List<Tween>();
    }

    public void Initialize()
    {
        agentsGroup = gameContext.GetGroup(GameMatcher.Agent);
        roundStartedGroup = gameContext.GetGroup(GameMatcher.RoundStarted);
        roundFinishedGroup = gameContext.GetGroup(GameMatcher.RoundFinished);
        effectsGroup = gameContext.GetGroup(GameMatcher.Effect);
        enemyGroup = gameContext.GetGroup(GameMatcher.Enemy);

        enemyGroup.OnEntityAdded += OnSafePointsInfoAvailable;

        roundStartedGroup.OnEntityAdded += OnRoundStarted;
        roundFinishedGroup.OnEntityAdded += OnRoundFinished;
    }

    //ToDo: safe points data should be probably moved away from enemy
    //one time action to create repair points
    private void OnSafePointsInfoAvailable(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        enemyGroup.OnEntityAdded -= OnSafePointsInfoAvailable;

        var stationaryPoints = entity.aIPerception.stationaryPositions;

        //pick every 2 hiding points
        for(int i = 0; i<stationaryPoints.Length; i+=2)
        {
            var position = stationaryPoints[i];
            position.y = 1; //just more visually pleasing [height]

            effectsFactory.CreateEffect(EntityPrefabNameBinding.EFFECT_PERSISTANT_ADD_HEALTH_BINDING, position);
        }
    }



    private void OnEffectCollectedDuringRound(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        spawnDelayedCalls.Add(DOVirtual.DelayedCall(60 * gameContext.match.random.value, CreateEffect));
    }

    private void OnRoundFinished(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        effectsGroup.OnEntityRemoved -= OnEffectCollectedDuringRound;

        foreach (var effectEntity in effectsGroup.GetEntities())
        {
            //only collectible effects are respawned
            if(effectEntity.effect.effect.IsCollectible())
            {
                effectEntity.Destroy();
            }
        }

        foreach(var delayedCall in spawnDelayedCalls)
        {
            delayedCall.Kill();
        }
        spawnDelayedCalls.Clear();
    }

    private void OnRoundStarted(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        effectsGroup.OnEntityRemoved += OnEffectCollectedDuringRound;

        int requiredEffects = gameContext.match.effectsAtTimeCap;

        for(int i = 0; i<requiredEffects; i++)
        {
            spawnDelayedCalls.Add(DOVirtual.DelayedCall(60 * gameContext.match.random.value, CreateEffect));
        }
    }

    private void CreateEffect()
    {
        var randomIndex = Mathf.FloorToInt(gameContext.match.random.value * spawnableEffects.Length) % spawnableEffects.Length;

        var prefabBinding = spawnableEffects[randomIndex];

        var randomPosition = gameContext.match.random.insideUnitCircle * 10;

        var position = new Vector3(randomPosition.x, 1, randomPosition.y); //transforming 2d to 3d random position

        effectsFactory.CreateEffect(prefabBinding, position);
    }

    public void Execute()
    {
        foreach (var agentEntity in agentsGroup.GetEntities())
        {
            var agentsEffects = agentEntity.agent.effects;

            foreach (var effect in agentsEffects)
            {
                effect.Update(inputContext.tick.currentTick);

                effect.Apply(agentEntity);
                
                if(effect.IsUsed())
                {
                    effectsToRemove.Add(effect);
                }
            }

            //cleanup
            //these could be potentially pooled as well
            foreach (var effect in effectsToRemove)
            {
                agentsEffects.Remove(effect);
            }
            effectsToRemove.Clear();
        }
    }
}

