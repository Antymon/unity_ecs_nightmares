﻿using DG.Tweening;
using NUnit.Framework;

public class EntitasTestSuite : IEntityDeserializer {

    private Feature systems;
    protected GameContext gameContext;
    protected InputContext inputContext;

	[SetUp]
	public void CollisionSystemTestSuiteSetup() 
    {
        systems = new Feature("Systems");

        gameContext = new GameContext();
        inputContext = new InputContext();

        var dummyEntityDeserializer = this;

        systems.Add(new DestroySystem(gameContext));
        systems.Add(new EffectTriggerSystem(inputContext));
        systems.Add(new CollisionSystem(inputContext, gameContext));
        systems.Add(new PlayerControlsSystem(inputContext, gameContext, dummyEntityDeserializer));

        systems.Initialize();
    }

    protected void UpdateSystems()
    {
        systems.Execute();
        systems.Cleanup();
    }

    [TearDown]
    public void TearDown()
    {
        DOTween.KillAll();

        systems.TearDown();

        systems.ClearReactiveSystems();

        gameContext.DestroyAllEntities();
        inputContext.DestroyAllEntities();

        systems = null;
    }

    protected GameEntity CreateGameEntity()
    {
        return gameContext.CreateEntity();
    }

    protected InputEntity CreateInputEntity()
    {
        return inputContext.CreateEntity();
    }

    public void DeserializeEnitity(GameEntity entity)
    {
        //just a dummy to satisfy the interface
    }
}
