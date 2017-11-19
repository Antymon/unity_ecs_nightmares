using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBulletSystem : ReactiveSystem<InputEntity>, IInitializeSystem
{
    private IEntityDeserializer entityDeserializer;

    private GameContext gameContext;

    private GameEntity playerEntity;

    private PlayerGunTriggerManager triggerManager;
    private JoypadManager joypadProcessor;

    public TriggerBulletSystem(InputContext context, GameContext gameContext, IEntityDeserializer deserializer)
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

        triggerManager = new PlayerGunTriggerManager(joypadEntity);
        joypadProcessor = new JoypadManager(joypadEntity);
    }

    private void OnPlayerDestroyed(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        joypadProcessor.Disable();

        playerEntity = null;
        triggerManager.playerEntity = null;
        joypadProcessor.playerEnity = null;
    }

    private void OnPlayerCreated(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        playerEntity = entity;
        triggerManager.playerEntity = playerEntity;
        joypadProcessor.playerEnity = playerEntity;
    }

    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        if (playerEntity != null && playerEntity.isEnabled)
        {
            joypadProcessor.ManageState(entities);

            //assumption: shooting is complementing navigation
            //specificically shooting is triggered by second touch point wheras navigation by first
            //so if navigation is disabled, nothing to consider
            if (joypadProcessor.IsEnabled())
            {
                triggerManager.ManageState(entities);
            }
            else
            {
                triggerManager.Disable();
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

