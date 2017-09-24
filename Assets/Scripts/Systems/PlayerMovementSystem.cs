using Entitas;

public class PlayerMovementSystem : ReactiveSystem<GameEntity>
{

    public PlayerMovementSystem(GameContext context)
        : base(context)
    {

    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.movementDirectionChangedListener.listener.OnMovementDirectionChanged(entity.movementDirection.direction);
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isPlayer && entity.hasMovementDirectionChangedListener;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.MovementDirection.Added());
    }
}

