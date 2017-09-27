using UnityEngine;
using Entitas;

//signalizes instance-vlass relationship for a game object
public interface IPrefabIdentifier
{
    EntityPrefabNameBinding GetPrefabBinding();
}

//signalizes game object-entity relationship
public interface IEntitasBinding : IPrefabIdentifier, IPooledGameObject, IEntityDeserializer
{
    //returns entity associated with game object
    GameEntity GetEntity();
}