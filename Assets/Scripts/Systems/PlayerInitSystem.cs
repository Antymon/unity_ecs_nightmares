using Entitas;
using System.Collections.Generic;

public class PlayerInitSystem  : IInitializeSystem
{
    private GameContext context;
    private IEntityDeserializer entityDeserializer;

    public PlayerInitSystem(GameContext context, IEntityDeserializer entityDeserializer)
    {
        this.context = context;
        this.entityDeserializer = entityDeserializer;
    }

    public void Initialize()
    {
        var entity = context.CreateEntity();
        entity.isPlayer = true;
        entity.AddEntityBinding(EntityPrefabNameBinding.PLAYER_BINDING);
        entity.AddAgent(0, string.Empty, new List<IEffect>());
        entityDeserializer.DeserializeEnitity(entity);

    }

}

