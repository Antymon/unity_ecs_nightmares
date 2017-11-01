using NUnit.Framework;

public class EntitasTestSuite {

    private Feature systems;

	[SetUp]
	public void CollisionSystemTestSuiteSetup() 
    {
        systems = new Feature("Systems");

        var gameContext = Contexts.sharedInstance.game;
        var inputContext = Contexts.sharedInstance.input;

        systems.Add(new DestroySystem(gameContext));
        systems.Add(new CollisionSystem(inputContext, gameContext));
        systems.Initialize();
    }

    protected void UpdateSystems()
    {
        systems.Execute();
        systems.Cleanup();
    }

    protected GameEntity CreateGameEntity()
    {
        return Contexts.sharedInstance.game.CreateEntity();
    }

    protected InputEntity CreateInputEntity()
    {
        return Contexts.sharedInstance.input.CreateEntity();
    }
}
