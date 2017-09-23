using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTriggerBehaviour : AbstractEntitasBehaviour {

    private BackgroundEntity backgroundEntity;

    override public EntityPrefabNameBinding GetPrefabBinding()
    {
        return EntityPrefabNameBinding.BACKGROUND_BINDING;
    }

    protected override void OnDestroyHandler(Entitas.IEntity entity)
    {
        throw new UnityException("Background are not expected to be destroyed");
    }

    public override void DeserializeEnitity(Entitas.Entity entity)
    {
        base.DeserializeEnitity(entity);
        backgroundEntity = (BackgroundEntity)entity;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var entitasBinding = other.gameObject.GetComponent<IEntitasBinding>();

        if (entitasBinding == null)
        {
            return;
        }

        var entity = entitasBinding.GetEntity();

        backgroundEntity.AddTrigger(entity);
    }
}
