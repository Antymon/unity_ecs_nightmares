using Entitas;
using UnityEngine;

public class BallMovementSystem : ReactiveSystem<BallEntity>
{
    public BallMovementSystem() : base(Contexts.sharedInstance.ball)
    {

    }

    protected override void Execute(System.Collections.Generic.List<BallEntity> entities)
    {
        foreach (var entity in entities)
        {
            var collision = entity.processedCollision;

            var ballVelocity = collision.collision.otherVelocity;

            var ballPositionTracker = entity.positionTracker;

            var ballDirection = -ballPositionTracker.trackedPositions.Last() + ballPositionTracker.trackedPositions.First();

            var ballReflected = Vector2.Reflect(ballDirection, entity.processedCollision.collision.collision2D.contacts[0].normal);

            ballReflected.Normalize();

            ballReflected += entity.processedCollision.collision.additionalForce;

            ballReflected.Normalize();

            entity.ballChangedDirectionListener.listener.DirectionChanged(ballReflected);

            entity.RemoveProcessedCollision();
        }
    }

    protected override bool Filter(BallEntity entity)
    {
        return true;
    }

    protected override ICollector<BallEntity> GetTrigger(IContext<BallEntity> context)
    {
        return context.CreateCollector<BallEntity>(BallMatcher.ProcessedCollision.Added());
    }
}

