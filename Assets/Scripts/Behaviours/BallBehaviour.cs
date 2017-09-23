using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : AbstractEntitasBehaviour, BallChangedDirectionListener {

    private BallEntity ballEntity;
    private Vector3 initialPosition;

    public void Start()
    {
        initialPosition = transform.position;
    }

    override public EntityPrefabNameBinding GetPrefabBinding()
    {
        return EntityPrefabNameBinding.BALL_BINDING;
    }

    public override void DeserializeEnitity(Entitas.Entity entity)
    {
        base.DeserializeEnitity(entity);
        ballEntity = ((BallEntity)entity);
        ballEntity.AddBallChangedDirectionListener(this);
    }

    override public void Reset()
    {
        base.Reset();
        transform.position = initialPosition;
        ballEntity = null;
    }

    public void Update()
    {
        ballEntity.positionTracker.trackedPositions.Add(transform.position);
    }

    public void DirectionChanged(Vector2 direction)
    {
        var rigidbody2d = this.GetComponent<Rigidbody2D>();

        if(rigidbody2d == null)
        {
            return;
        }

        rigidbody2d.velocity = direction;
    }
}
