using UnityEngine;
using Entitas;
public interface IPrefabIdentifier
{
    EntityPrefabNameBinding GetPrefabBinding();
}

public interface IEntitasBinding : IPrefabIdentifier, IPooledObject, IEntityDeserializer
{
    Entity GetEntity();
}