using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;

//using interface as a method of hiding specifics of the implementation
public interface IEntityDeserializer
{
    void DeserializeEnitity(Entity entity);
}

//deserialization is bound to data set on GameObjects
//interface inverts the dependency though so that entitas side remains uaware of dependency
//whenever agnostic execution is required - new implementation of the deserialization needs to be provided
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

