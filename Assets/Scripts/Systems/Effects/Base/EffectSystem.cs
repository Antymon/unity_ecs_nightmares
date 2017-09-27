using Entitas;

public class EffectSystem : IInitializeSystem, IExecuteSystem
{
    private GameContext context;
    private IEntityDeserializer entityDeserializer;

    private IGroup<GameEntity> effectsGroup;
    private IGroup<GameEntity> agentsGroup;

    private IEffectsFactory effectsFactory;


    public EffectSystem(GameContext context, IEntityDeserializer entityDeserializer)
    {
        this.context = context;
        this.entityDeserializer = entityDeserializer;

        effectsFactory = new EffectsFactory();
    }

    public void Initialize()
    {
        effectsGroup = context.GetGroup(GameMatcher.Effect);
        effectsGroup.OnEntityAdded += OnEffectEntityCreated;

        agentsGroup = context.GetGroup(GameMatcher.Agent);
    }

    private void OnEffectEntityCreated(IGroup<GameEntity> group, GameEntity effectEntity, int index, IComponent component)
    {
        EffectComponent effectComponent = effectEntity.effect;
        var perfabBinding = effectEntity.entityBinding;

        if(perfabBinding.Equals(EntityPrefabNameBinding.EFFECT_ADD_HEALTH_BINDING))
        {
            effectComponent.effect = effectsFactory.Create<AddHealthEffect>();
        }

        entityDeserializer.DeserializeEnitity(effectEntity);
    }

    public void Execute()
    {
        foreach (var agentEntity in agentsGroup.GetEntities())
        {
            foreach(var effect in agentEntity.agent.effects)
            {
                //manage effect
                //apply, ignore or remove

                if(effect.CanApply(agentEntity))
                {
                    effect.Apply(agentEntity);
                }
                
                if(effect.IsUsed())
                {
                    agentEntity.agent.effects.Remove(effect);
                }

                effect.Update();
            }
        }
    }
}

