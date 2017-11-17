using System;
using System.Collections.Generic;
using UnityEngine;

//meant to create shortlived signals
//signals live only at their dispatch cycle
//next cycle they are recycled as regular entities

public interface ISignalEntityFactory
{
    GameEntity Create();
}

public class SignalEntityFactory : ISignalEntityFactory
{
    private GameContext gameContext;

    public SignalEntityFactory()
    {
        this.gameContext = Contexts.sharedInstance.game;
    }

    public GameEntity Create()
    {
        var signalEntity = gameContext.CreateEntity();
        signalEntity.isMarkedToPostponedDestroy = true;
        return signalEntity;
    }
}
