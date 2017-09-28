using Entitas;
using System.Collections.Generic;

public class EnemyInitSystem  : IInitializeSystem
{
    private GameContext gameContext;
    private InputContext inputContext;
    private IEntityDeserializer entityDeserializer;


    public EnemyInitSystem(GameContext gameContext, InputContext inputContext, IEntityDeserializer entityDeserializer)
    {
        this.gameContext = gameContext;
        this.inputContext = inputContext;
        this.entityDeserializer = entityDeserializer;
    }

    public void Initialize()
    {

    }

}

