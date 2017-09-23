using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityDeserializer
{
    void DeserializeEnitity(Entity entity);
}

public class EntityDeserializerViaBinding : IEntityDeserializer
{
    private IPool pool;

    public EntityDeserializerViaBinding(IPool pool)
    {
        this.pool = pool;
    }

    public void DeserializeEnitity(Entity entity)
    {
        var entityType = entity.GetType();

        var prefabName = EntityPrefabNameBinding.entityTypeToPrefabName[entityType].prefabName;

        var relatedGO = pool.Get(prefabName);

        var deserializers = relatedGO.GetComponents<IEntityDeserializer>();

        foreach (var deserializer in deserializers)
        {
            deserializer.DeserializeEnitity(entity);
        }
    }
}

