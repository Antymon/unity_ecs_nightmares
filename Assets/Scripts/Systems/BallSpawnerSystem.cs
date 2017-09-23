using Entitas;
using UnityEngine;

public class BallSpawnerSystem : ReactiveSystem<BallEntity>, IInitializeSystem
{
    public BallContext ballContext;
    private IEntityDeserializer deserializer;

    public BallSpawnerSystem(BallContext ballContext, IEntityDeserializer entityDeserializer)
        : base(ballContext)
    {
        this.ballContext = ballContext;
        this.deserializer = entityDeserializer;
    }

    public void Initialize()
    {
        var entity = ballContext.CreateEntity();
        entity.AddPositionTracker(new SinkingQueue<Vector2>());

        deserializer.DeserializeEnitity(entity);

        entity.ballChangedDirectionListener.listener.DirectionChanged(Vector2.down);
    }

    protected override void Execute(System.Collections.Generic.List<BallEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.Destroy();
        }

        Initialize();
    }

    protected override bool Filter(BallEntity entity)
    {
        return true;
    }

    protected override ICollector<BallEntity> GetTrigger(IContext<BallEntity> context)
    {
        return context.CreateCollector<BallEntity>(BallMatcher.BallOut.Added());
    }
}

