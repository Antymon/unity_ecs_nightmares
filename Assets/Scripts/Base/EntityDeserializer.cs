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
    private IGameObjectPool pool;

    public EntityDeserializerViaBinding(IGameObjectPool pool)
    {
        this.pool = pool;
    }

    public void DeserializeEnitity(GameEntity entity)
    {
        var entityType = entity.entityBinding.entitasBinding.entityType;

        var id = EntityPrefabNameBinding.entityTypeToBinding[entityType].id;

        var relatedGO = pool.Get(id);

        var deserializers = relatedGO.GetComponents<IEntityDeserializer>();

        foreach (var deserializer in deserializers)
        {
            deserializer.DeserializeEnitity(entity);
        }
    }
}

