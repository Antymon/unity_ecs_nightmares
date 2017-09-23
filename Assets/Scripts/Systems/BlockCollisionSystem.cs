using Assets.Scripts.Components;
using Entitas;

public class BlockCollisionSystem : ReactiveSystem<BlockEntity>, IInitializeSystem
{
    private IEntityDeserializer entityDeserializer;

    public BlockCollisionSystem(IEntityDeserializer entityDeserializer):base(Contexts.sharedInstance.block)
    {
        this.entityDeserializer = entityDeserializer;
    }

    protected override void Execute(System.Collections.Generic.List<BlockEntity> entities)
    {
        foreach (var entity in entities)
        {
            var otherEntity = entity.collision.other;

            ICollisionInfo blockCollisionInfo = new BlockCollisionInfo(entity.collision.collision, entity.collision.velocity);

            if (otherEntity is BallEntity)
            {
                ((BallEntity)otherEntity).ReplaceProcessedCollision(blockCollisionInfo);
            }

            entity.Destroy();
        }
    }

    protected override bool Filter(BlockEntity entity)
    {
        return true;
    }

    protected override ICollector<BlockEntity> GetTrigger(IContext<BlockEntity> context)
    {
        return context.CreateCollector<BlockEntity>(BlockMatcher.Collision.Added());
    }

    public void Initialize()
    {

        //hardcoded level data
        for (int i = 0; i < 4; i++)
            entityDeserializer.DeserializeEnitity(Contexts.sharedInstance.block.CreateEntity());
    }
}

