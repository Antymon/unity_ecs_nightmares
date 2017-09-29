using Entitas;
using UnityEngine;

public interface IEffectsFactory
{
    GameEntity CreateEffect(EntityPrefabNameBinding prefabBinding, Vector3 position);
}

public class EffectsFactory : IEffectsFactory
{
    private GameContext gameContext;
    private IEntityDeserializer entityDeserializer;

    public EffectsFactory(GameContext gameContext, IEntityDeserializer entityDeserializer)
    {
        this.gameContext = gameContext;
        this.entityDeserializer = entityDeserializer;
    }

    public GameEntity CreateEffect(EntityPrefabNameBinding prefabBinding, Vector3 position)
    {
        var effectEntity = gameContext.CreateEntity();
        effectEntity.AddEntityBinding(prefabBinding);
        effectEntity.AddPosition(position);

        //could be folded nicely with reflaction and mapping
        if (prefabBinding.Equals(EntityPrefabNameBinding.EFFECT_ADD_HEALTH_BINDING))
        {
            effectEntity.AddEffect(Create<AddHealthEffect>());
        }
        else if (prefabBinding.Equals(EntityPrefabNameBinding.EFFECT_MOVEMENT_INVERTER_BINDING))
        {
            effectEntity.AddEffect(Create<MovementInverterEffect>());
        }
        else if (prefabBinding.Equals(EntityPrefabNameBinding.EFFECT_PERSISTANT_ADD_HEALTH_BINDING))
        {
            effectEntity.AddEffect(Create<PersistantAddHealthEffect>());
        }

        entityDeserializer.DeserializeEnitity(effectEntity);

        return effectEntity;
    }

    private IEffect Create<T>() where T : IEffect, new()
    {
        return new T();
    }
}

