using Entitas;
using System.Collections.Generic;

public class PlayerInitSystem  : IInitializeSystem
{
    private GameContext gameContext;
    private InputContext inputContext;
    private IEntityDeserializer entityDeserializer;

    private IGroup<GameEntity> creationRequestGroup;

    public PlayerInitSystem(GameContext gameContext, InputContext inputContext, IEntityDeserializer entityDeserializer)
    {
        this.gameContext = gameContext;
        this.inputContext = inputContext; 
        this.entityDeserializer = entityDeserializer;
    }

    public void Initialize()
    {
        creationRequestGroup = gameContext.GetGroup(GameMatcher.CreationRequest);
        creationRequestGroup.OnEntityAdded += OnCreationRequest;
    }

    //request will be promoted to actual entity if binding is correct
    private void OnCreationRequest(IGroup<GameEntity> group, GameEntity requestEntity, int index, IComponent component)
    {

    }
}

