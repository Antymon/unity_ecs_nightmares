using Entitas;

public class EnemyInitSystem  : IInitializeSystem
{
    private GameContext context;
    private IEntityDeserializer entityDeserializer;

    public EnemyInitSystem(GameContext context, IEntityDeserializer entityDeserializer)
    {
        this.context = context;
        this.entityDeserializer = entityDeserializer;
    }

    public void Initialize()
    {
        var entity = context.CreateEntity();
        entity.AddEntityBinding(EntityPrefabNameBinding.ENEMY_BINDING);
        entityDeserializer.DeserializeEnitity(entity);
    }

}

