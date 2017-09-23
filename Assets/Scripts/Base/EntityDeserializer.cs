using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;

//using interface as a method of hiding specifics of the implementation
public interface IEntityDeserializer
{
    void DeserializeEnitity(GameEntity entity);
}

//this deserializer supports entities instances with data set on corresponding GameObjects
//interface inverts the dependency so that Entitas side remains uaware of dependency on Unity APIs
//whenever agnostic execution is required - new implementation of the deserialization needs to be provided
public class EntityDeserializerViaBinding : IEntityDeserializer
{
    private IPool pool;

    public EntityDeserializerViaBinding(IPool pool)
    {
        this.pool = pool;
    }

    public void DeserializeEnitity(GameEntity entity)
    {
        var entityType = entity.entityBinding.entitasBinding.entityType;

        var prefabName = EntityPrefabNameBinding.entityTypeToPrefabName[entityType].prefabName;

        var relatedGO = pool.Get(prefabName);

        var deserializers = relatedGO.GetComponents<IEntityDeserializer>();

        foreach (var deserializer in deserializers)
        {
            deserializer.DeserializeEnitity(entity);
        }
    }
}

