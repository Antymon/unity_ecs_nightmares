using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class JoypadSystem : ReactiveSystem<InputEntity>, IInitializeSystem
{
    //stateless utility
    public static float GetAngleFromDirection(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    }

    public GameEntity playerEnity;
    private IEntityDeserializer entityDeserializer;
    private GameContext gameContext;
    private JoypadManager joypadProcessor;

    public JoypadSystem(InputContext context, GameContext gameContext, IEntityDeserializer deserializer)
        : base(context)
    {
        this.entityDeserializer = deserializer;
        this.gameContext = gameContext;
    }

    //create joypad binding, fetch player entity
    public void Initialize()
    {
        var joypadEntity = gameContext.CreateEntity();
        joypadEntity.AddEntityBinding(EntityPrefabNameBinding.JOYPAD_BINDING);
        joypadEntity.AddJoypad(newEnabled: false, newTouchId:-1);
        entityDeserializer.DeserializeEnitity(joypadEntity);

        var playerGroup = gameContext.GetGroup(GameMatcher.Player);
        playerGroup.OnEntityAdded += OnPlayerCreated;

        joypadProcessor = new JoypadManager(joypadEntity);
    }

    private void OnPlayerCreated(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        playerEnity = entity;
        playerEnity.OnDestroyEntity += OnPlayerDestroyed;

        joypadProcessor.playerEnity = playerEnity;
    }

    private void OnPlayerDestroyed(IEntity entity)
    {
        playerEnity.OnDestroyEntity -= OnPlayerDestroyed;

        joypadProcessor.Disable();
    }

    //ToDo: consider generalization of Trigger and Joypad systems
    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        if (playerEnity == null || !playerEnity.isEnabled)
        {
            foreach (var entity in entities)
            {
                entity.Destroy();
            }
            return;
        }

        joypadProcessor.ManageState(entities);
    }

    protected override bool Filter(InputEntity entity)
    {
        return true;
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector<InputEntity>(InputMatcher.Touches.Added());
    }
}

