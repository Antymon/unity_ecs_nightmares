using Entitas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehaviour : MonoBehaviour, IEntityDeserializer
{
    GameEntity entity;

    void OnCollisionEnter(Collision coll)
    {

    }

    public void DeserializeEnitity(GameEntity entity)
    {
        this.entity = entity;
    }

    void OnTriggerEnter(Collider other)
    {
        ReportTrigger(other.gameObject, true);
    }

    void OnTriggerExit(Collider other)
    {
        ReportTrigger(other.gameObject, false);
    }

    private void ReportTrigger(GameObject gameObject, bool onEnter)
    {
        var entitasBinding = gameObject.GetComponent<IEntitasBinding>();

        if (entitasBinding == null)
        {
            return;
        }

        Contexts.sharedInstance.input.CreateEntity().ReplaceTrigger(entity, entitasBinding.GetEntity(), onEnter);
    }
}
