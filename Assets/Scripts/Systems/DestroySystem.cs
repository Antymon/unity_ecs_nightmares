using Entitas;
using System.Collections.Generic;

public class DestroySystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    private HashSet<GameEntity> entitiesToDestroy;

    public DestroySystem(GameContext context)
        : base(context)
    {
        entitiesToDestroy = new HashSet<GameEntity>();
    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entitiesToDestroy.Add(entity);
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isMarkedToPostponedDestroy;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.MarkedToPostponedDestroy.Added());
    }

    public void Cleanup()
    {
        foreach (var entity in entitiesToDestroy)
        {
            entity.Destroy();
        }

        entitiesToDestroy.Clear();
    }
}

