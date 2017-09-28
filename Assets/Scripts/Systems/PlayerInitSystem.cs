using Entitas;
using System.Collections.Generic;

public class PlayerInitSystem  : IInitializeSystem
{
    private GameContext gameContext;
    private InputContext inputContext;
    private IEntityDeserializer entityDeserializer;

    public PlayerInitSystem(GameContext gameContext, InputContext inputContext, IEntityDeserializer entityDeserializer)
    {
        this.gameContext = gameContext;
        this.inputContext = inputContext; 
        this.entityDeserializer = entityDeserializer;
    }

    public void Initialize()
    {
        var entity = gameContext.CreateEntity();
        entity.isPlayer = true;
        entity.AddEntityBinding(EntityPrefabNameBinding.PLAYER_BINDING);
        //ToDo: opponent as null is not desirable
        entity.AddAgent(0, string.Empty, new List<IEffect>(),null);
        

        entityDeserializer.DeserializeEnitity(entity);

        entity.AddPositionChanged(inputContext.tick.currentTick, 0, false, entity.position.position);
    }

}

