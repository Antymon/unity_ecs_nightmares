using Entitas;

public class MovementSystem : IInitializeSystem, IExecuteSystem
{
    private GameContext gameContext;
    private InputContext inputContext;

    private IGroup<GameEntity> movementDestinationGroup;
    private IGroup<GameEntity> movementDirectionGroup;
    //private ICollector<GameEntity> positionCollector;
    private IGroup<GameEntity> positionGroup;
    private PositionChangedComponent positionChangedComponent;
    private GameEntity[] entities;

    public MovementSystem(GameContext context, InputContext inputContext)
    {
        this.gameContext = context;
        this.inputContext = inputContext;
    }

    public void Initialize()
    {
        movementDestinationGroup = gameContext.GetGroup(GameMatcher.MovementDestination);
        movementDestinationGroup.OnEntityUpdated += OnMovementDestinationChanged;

        movementDirectionGroup = gameContext.GetGroup(GameMatcher.MovementDirection);
        movementDirectionGroup.OnEntityUpdated += OnMovementDirectionChanged;

        positionGroup = gameContext.GetGroup(GameMatcher.Position);
        //positionCollector = positionGroup.CreateCollector(GroupEvent.Added);

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

    public void Execute()
    {   
        entities = positionGroup.GetEntities();
        foreach(var entity in entities)
        {
            if(!entity.hasPositionChanged)
            {
                continue;
            }

            positionChangedComponent = entity.positionChanged;

            if ((entity.position.position - positionChangedComponent.lastPositon).sqrMagnitude < .01f)
            {
                positionChangedComponent.ticksStationary++;
            }
            else
            {
                entity.ReplacePositionChanged(inputContext.tick.currentTick, newTicksStationary: 0, newLastPositon: entity.position.position);
            }
        }
    }
}

