using Assets.Scripts.Components;
using Entitas;

public class PadCollisionSystem : ReactiveSystem<PadEntity>, IInitializeSystem
{
    private IEntityDeserializer entityDeserializer;

    public PadCollisionSystem(IEntityDeserializer entityDeserializer)
        : base(Contexts.sharedInstance.pad)
    {
        this.entityDeserializer = entityDeserializer;
    }

    protected override void Execute(System.Collections.Generic.List<PadEntity> entities)
    {
        foreach (var entity in entities)
        {
            var padPositionTracker = entity.positionTracker;

            var padDirection = -padPositionTracker.trackedPositions.Last() + padPositionTracker.trackedPositions.First();

            padDirection.Normalize();

            var otherEntity = entity.collision.other;

            ICollisionInfo padCollisionInfo = new PadCollisionInfo(entity.collision.collision, padDirection, entity.collision.velocity);

            if(otherEntity is BallEntity)
            {
                ((BallEntity)otherEntity).ReplaceProcessedCollision(padCollisionInfo);
            }

            entity.RemoveCollision();
        }
    }

    protected override bool Filter(PadEntity entity)
    {
        return true;
    }

    protected override ICollector<PadEntity> GetTrigger(IContext<PadEntity> context)
    {
        return context.CreateCollector<PadEntity>(PadMatcher.Collision.Added());
    }

    public void Initialize()
    {
        //two pads are by definition already added to the stage
        CreatePadEntity();
        CreatePadEntity();
    }

    private void CreatePadEntity()
    {
        var pad1 = Contexts.sharedInstance.pad.CreateEntity();
        pad1.AddPositionTracker(new SinkingQueue<UnityEngine.Vector2>());
        pad1.AddKeyInput(0, 0);

        entityDeserializer.DeserializeEnitity(pad1);
    }
}

