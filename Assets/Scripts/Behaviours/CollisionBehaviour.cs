using Entitas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBehaviour : MonoBehaviour, IEntityDeserializer
{
    Entity entity;

    void OnCollisionEnter(Collision coll)
    {
        var entitasBinding = coll.gameObject.GetComponent<IEntitasBinding>();

        if (entitasBinding == null)
        {
            return;
        }

        Contexts.sharedInstance.input.CreateEntity().ReplaceCollision(entity, entitasBinding.GetEntity());
    }

    public void DeserializeEnitity(Entity entity)
    {
        this.entity = entity;
    }
}
