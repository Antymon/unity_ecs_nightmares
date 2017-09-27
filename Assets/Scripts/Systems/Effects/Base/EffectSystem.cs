using Entitas;
using System.Collections.Generic;

public class EffectSystem : IInitializeSystem, IExecuteSystem
{
    private GameContext context;
    private IEntityDeserializer entityDeserializer;

    private IGroup<GameEntity> creationRequestGroup;
    private IGroup<GameEntity> agentsGroup;

    private IEffectsFactory effectsFactory;

    private List<IEffect> effectsToRemove;

    public EffectSystem(GameContext context, IEntityDeserializer entityDeserializer)
    {
        this.context = context;
        this.entityDeserializer = entityDeserializer;

        effectsFactory = new EffectsFactory();

        effectsToRemove = new List<IEffect>();
    }

    public void Initialize()
    {
        creationRequestGroup = context.GetGroup(GameMatcher.CreationRequest);
        creationRequestGroup.OnEntityAdded += OnCreationRequest;

        agentsGroup = context.GetGroup(GameMatcher.Agent);
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
                //manage effect
                //apply, ignore or remove

                if(effect.CanApply(agentEntity))
                {
                    effect.Apply(agentEntity);
                }
                
                if(effect.IsUsed())
                {
                    effectsToRemove.Add(effect);
                }

                effect.Update();
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

