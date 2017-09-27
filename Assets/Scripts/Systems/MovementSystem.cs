using Entitas;
using UnityEngine;

public class MovementSystem : IInitializeSystem
{
    private GameContext context;

    private IGroup<GameEntity> movementDestinationGroup;
    private IGroup<GameEntity> movementDirectionGroup;

    public MovementSystem(GameContext context)
    {
        this.context = context;
    }

    public void Initialize()
    {
        movementDestinationGroup = context.GetGroup(GameMatcher.MovementDestination);
        movementDestinationGroup.OnEntityUpdated += OnMovementDestinationChanged;

        movementDirectionGroup = context.GetGroup(GameMatcher.MovementDirection);
        movementDirectionGroup.OnEntityUpdated += OnMovementDirectionChanged;
    }

    private void OnMovementDestinationChanged(IGroup<GameEntity> group, GameEntity entity, int index, IComponent previousComponent, IComponent newComponent)
    {
        if (entity.hasMovementDestinationChangedListener)
        {
            IMovementDestinationChangedListener listener = entity.movementDestinationChangedListener.listener;
            listener.OnMovementDestinationChanged(entity.movementDestination.destination);
            listener.OnOrientationDestinationChanged(entity.movementDestination.orientation);
        }
    }

    private void OnMovementDirectionChanged(IGroup<GameEntity> group, GameEntity entity, int index, IComponent previousComponent, IComponent newComponent)
    {
        if(entity.hasMovementDirectionChangedListener)
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
}

