using UnityEngine;
using Entitas;

//signalizes instance-vlass relationship for a game object
public interface IPrefabIdentifier
{
    EntityPrefabNameBinding GetPrefabBinding();
}

//signalizes game object-entity relationship
public interface IEntitasBinding : IPrefabIdentifier, IPooledObject, IEntityDeserializer
{
    //returns entity associated with game object
    Entity GetEntity();
}