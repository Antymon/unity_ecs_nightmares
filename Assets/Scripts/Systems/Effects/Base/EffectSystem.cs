using Entitas;
using System.Collections.Generic;

public class EffectSystem : IInitializeSystem, IExecuteSystem
{
    private GameContext gameContext;
    private InputContext inputContext;
    private IEntityDeserializer entityDeserializer;

    private IGroup<GameEntity> creationRequestGroup;
    private IGroup<GameEntity> agentsGroup;

    private IEffectsFactory effectsFactory;

    private List<IEffect> effectsToRemove;

    public EffectSystem(GameContext gameContext,InputContext inputContext, IEntityDeserializer entityDeserializer)
    {
        this.gameContext = gameContext;
        this.inputContext = inputContext;
        this.entityDeserializer = entityDeserializer;

        effectsFactory = new EffectsFactory();

        effectsToRemove = new List<IEffect>();
    }

    public void Initialize()
    {
        creationRequestGroup = gameContext.GetGroup(GameMatcher.CreationRequest);
        creationRequestGroup.OnEntityAdded += OnCreationRequest;

        agentsGroup = gameContext.GetGroup(GameMatcher.Agent);
    }

    //request will be promoted to actual entity if binding is correct
    private void OnCreationRequest(IGroup<GameEntity> group, GameEntity requestEntity, int index, IComponent component)
    {
        var perfabBinding = requestEntity.entityBinding.entitasBinding;

        //could be folded nicely with reflaction and mapping
        if(perfabBinding.Equals(EntityPrefabNameBinding.EFFECT_ADD_HEALTH_BINDING))
        {
            requestEntity.AddEffect(effectsFactory.Create<AddHealthEffect>());
        }
        else if (perfabBinding.Equals(EntityPrefabNameBinding.EFFECT_MOVEMENT_INVERTER_BINDING))
        {
            requestEntity.AddEffect(effectsFactory.Create<MovementInverterEffect>());
        }
        else if (perfabBinding.Equals(EntityPrefabNameBinding.EFFECT_PERSISTANT_ADD_HEALTH_BINDING))
        {
            requestEntity.AddEffect(effectsFactory.Create<PersistantAddHealthEffect>());
        }
        else //not supported request
        {
            return;
        }

        //request is being promoted to actual object
        requestEntity.isCreationRequest = false;

        entityDeserializer.DeserializeEnitity(requestEntity);
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

