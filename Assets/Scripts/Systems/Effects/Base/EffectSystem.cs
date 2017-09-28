using DG.Tweening;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : IInitializeSystem, IExecuteSystem
{
    private GameContext gameContext;
    private InputContext inputContext;
    private IEntityDeserializer entityDeserializer;

    //private IGroup<GameEntity> creationRequestGroup;
    private IGroup<GameEntity> agentsGroup;
    private IGroup<GameEntity> roundStartedGroup;
    private IGroup<GameEntity> roundFinishedGroup;
    private IGroup<GameEntity> effectsGroup;

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
        this.entityDeserializer = entityDeserializer;

        effectsFactory = new EffectsFactory();

        effectsToRemove = new List<IEffect>();
        spawnDelayedCalls = new List<Tween>();
    }

    public void Initialize()
    {
        //creationRequestGroup = gameContext.GetGroup(GameMatcher.CreationRequest);
        //creationRequestGroup.OnEntityAdded += OnCreationRequest;

        agentsGroup = gameContext.GetGroup(GameMatcher.Agent);
        roundStartedGroup = gameContext.GetGroup(GameMatcher.RoundStarted);
        roundFinishedGroup = gameContext.GetGroup(GameMatcher.RoundFinished);
        effectsGroup = gameContext.GetGroup(GameMatcher.Effect);

        roundStartedGroup.OnEntityAdded += OnRoundStarted;
        roundFinishedGroup.OnEntityAdded += OnRoundFinished;
    }

    private void OnEffectCollected(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        spawnDelayedCalls.Add(DOVirtual.DelayedCall(60 * Random.value, CreateEffect));
    }

    private void OnRoundFinished(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        effectsGroup.OnEntityRemoved -= OnEffectCollected;

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

        entity.isMarkedToPostponedDestroy = true;
    }

    private void OnRoundStarted(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        effectsGroup.OnEntityRemoved += OnEffectCollected;

        int requiredEffects = gameContext.level.effectsAtTimeCap;

        for(int i = 0; i<requiredEffects; i++)
        {
            spawnDelayedCalls.Add(DOVirtual.DelayedCall(60 * Random.value, CreateEffect));
        }

        entity.isMarkedToPostponedDestroy = true;
    }

    private void CreateEffect()
    {
        var randomPosition = Random.insideUnitCircle * 10;
        var randomIndex = Mathf.FloorToInt(Random.value*spawnableEffects.Length)%spawnableEffects.Length;
        
        var prefabBinding = spawnableEffects[randomIndex];

        var effectEntity = gameContext.CreateEntity();
        effectEntity.AddEntityBinding(prefabBinding);
        effectEntity.AddPosition(new Vector3(randomPosition.x,1,randomPosition.y)); //transforming 2d to 3d random position

        //could be folded nicely with reflaction and mapping
        if(prefabBinding.Equals(EntityPrefabNameBinding.EFFECT_ADD_HEALTH_BINDING))
        {
            effectEntity.AddEffect(effectsFactory.Create<AddHealthEffect>());
        }
        else if (prefabBinding.Equals(EntityPrefabNameBinding.EFFECT_MOVEMENT_INVERTER_BINDING))
        {
            effectEntity.AddEffect(effectsFactory.Create<MovementInverterEffect>());
        }
        else //not supported request
        {
            return;
        }

        entityDeserializer.DeserializeEnitity(effectEntity);
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

