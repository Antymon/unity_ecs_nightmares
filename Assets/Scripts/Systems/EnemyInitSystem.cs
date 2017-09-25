using Entitas;
using System.Collections.Generic;

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
        entity.AddAgent(0, string.Empty, new List<IEffect>());

        var playerGroup = context.GetGroup(GameMatcher.Player);
        var playerEntity = playerGroup.GetSingleEntity();

        entity.AddEnemy(playerEntity);
        entityDeserializer.DeserializeEnitity(entity);
    }

}

