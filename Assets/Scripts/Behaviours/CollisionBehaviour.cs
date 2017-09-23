using Entitas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBehaviour : MonoBehaviour, IEntityDeserializer
{
    Entity entity;

    void OnCollisionEnter2D(Collision2D coll)
    {
        var entitasBinding = coll.gameObject.GetComponent<IEntitasBinding>();

        if (entitasBinding == null)
        {
            return;
        }

        var rigidBody2d = coll.gameObject.GetComponent<Rigidbody2D>();

        if (rigidBody2d == null)
        {
            return;
        }

        var otherEntity = entitasBinding.GetEntity();
        var velocity = rigidBody2d.velocity;

        Contexts.sharedInstance.input.CreateEntity().ReplaceCollision(coll, velocity, entity, otherEntity);
    }

    public void DeserializeEnitity(Entity entity)
    {
        this.entity = entity;
    }
}
