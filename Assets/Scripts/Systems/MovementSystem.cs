using Entitas;
using UnityEngine;

public class MovementSystem : ReactiveSystem<GameEntity>
{

    public MovementSystem(GameContext context)
        : base(context)
    {

    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var directionChangedListener = entity.movementDirectionChangedListener.listener;
            var direction = entity.movementDirection.direction;

            directionChangedListener.OnOrientationChanged(direction);

            if (!entity.movementDirection.onlyRotationAffected)
            {
                directionChangedListener.OnMovementDirectionChanged(direction);
            }
        }

        
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasMovementDirection && entity.hasMovementDirectionChangedListener;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.MovementDirection.Added());
    }
}

