using DG.Tweening;
using NUnit.Framework;

public class EntitasTestSuite : IEntityDeserializer {

    private Feature systems;
    private GameContext gameContext;
    private InputContext inputContext;

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
        systems.Add(new JoypadSystem(inputContext, gameContext, dummyEntityDeserializer));
        systems.Add(new TriggerBulletSystem(inputContext, gameContext));

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
