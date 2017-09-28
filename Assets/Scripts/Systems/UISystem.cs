using Entitas;

public class UISystem : IInitializeSystem
{
    private GameContext context;
    private IEntityDeserializer entityDeserializer;

    private GameEntity gameOverScreen;

    public UISystem(GameContext context, IEntityDeserializer entityDeserializer)
    {
        this.context = context;
        this.entityDeserializer = entityDeserializer;
    }

    public void Initialize()
    {
        context.GetGroup(GameMatcher.GameOver).OnEntityAdded += OnGameOver;

        gameOverScreen = CreateUIEntity(EntityPrefabNameBinding.GAME_OVER_SCREEN_BINDING);
    }

    private GameEntity CreateUIEntity(EntityPrefabNameBinding binding)
    {
        var entity = context.CreateEntity();
        entity.AddEntityBinding(EntityPrefabNameBinding.GAME_OVER_SCREEN_BINDING);
        entityDeserializer.DeserializeEnitity(entity);
        return entity;
    }

    private void OnGameOver(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        gameOverScreen.gameOverScreen.listener.OnShow();
    }


}

