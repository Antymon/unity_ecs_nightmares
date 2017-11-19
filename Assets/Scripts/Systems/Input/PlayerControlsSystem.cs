using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsSystem : ReactiveSystem<InputEntity>, IInitializeSystem
{
    private IEntityDeserializer entityDeserializer;

    private GameContext gameContext;

    private GameEntity playerEntity;

    private PlayerGunTriggerManager gunTriggerManager;
    private JoypadManager joypadManager;

    public PlayerControlsSystem(InputContext context, GameContext gameContext, IEntityDeserializer deserializer)
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
        joypadEntity.AddJoypad(newEnabled: false, newTouchId: -1);
        entityDeserializer.DeserializeEnitity(joypadEntity);

        var playerGroup = gameContext.GetGroup(GameMatcher.Player);
        playerGroup.OnEntityAdded += OnPlayerCreated;
        playerGroup.OnEntityRemoved += OnPlayerDestroyed;

        gunTriggerManager = new PlayerGunTriggerManager(joypadEntity);
        joypadManager = new JoypadManager(joypadEntity);
    }

    private void OnPlayerDestroyed(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        joypadManager.Disable();

        playerEntity = null;
        gunTriggerManager.playerEntity = null;
        joypadManager.playerEnity = null;
    }

    private void OnPlayerCreated(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        playerEntity = entity;
        gunTriggerManager.playerEntity = playerEntity;
        joypadManager.playerEnity = playerEntity;
    }

    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        if (playerEntity != null && playerEntity.isEnabled)
        {
            joypadManager.ManageState(entities);

            //assumption: shooting is complementing navigation
            //specificically shooting is triggered by second touch point wheras navigation by first
            //so if navigation is disabled, nothing to consider
            if (joypadManager.IsEnabled())
            {
                gunTriggerManager.ManageState(entities);
            }
            else
            {
                gunTriggerManager.Disable();
            }
        }

        foreach (var entity in entities)
        {
            entity.Destroy();
        }
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

