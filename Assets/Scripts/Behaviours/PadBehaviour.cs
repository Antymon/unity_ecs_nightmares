using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadBehaviour : AbstractEntitasBehaviour {

    private PadEntity padEntity; 

    override public EntityPrefabNameBinding GetPrefabBinding()
    {
        return EntityPrefabNameBinding.PAD_BINDING;
    }

    protected override void OnDestroyHandler(Entitas.IEntity entity)
    {
        throw new UnityException("Pads are not expected to be destroyed");
    }

    public override void DeserializeEnitity(Entitas.Entity entity)
    {
        base.DeserializeEnitity(entity);
        padEntity = ((PadEntity)entity);
    }

    public void Update()
    {
        padEntity.positionTracker.trackedPositions.Add(transform.position);
    }
}
