using Entitas;

public class JoypadInitSystem : IInitializeSystem
{
    private GameContext context;
    private IEntityDeserializer entityDeserializer;

    public JoypadInitSystem(GameContext context, IEntityDeserializer entityDeserializer)
    {
        this.context = context;
        this.entityDeserializer = entityDeserializer;
    }

    
    public void Initialize()
    {/*
        var entity = context.CreateEntity();
        entity.AddEntityBinding(EntityPrefabNameBinding.JOYPAD_BINDING);
        entity.AddJoystick(-1);
        entity.ReplaceJoystick(2);
        entityDeserializer.DeserializeEnitity(entity);
      * */
    }
}

