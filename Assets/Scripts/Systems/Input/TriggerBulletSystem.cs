using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBulletSystem : ReactiveSystem<InputEntity>, IInitializeSystem, ICleanupSystem
{
    private GameContext gameContext;
    private GameEntity joypadEntity;
    private GameEntity playerEntity;

    private PlayerGunTriggerManager triggerManager;
    //other systems are operating on overlapping subsets of entities
    //so cleanup has to be posponed after all executions have been made
    private List<InputEntity> entitiesCleanupRegister = new List<InputEntity>();

    public TriggerBulletSystem(InputContext context, GameContext gameContext)
        : base(context)
    {
        this.gameContext = gameContext;
    }

    public void Initialize()
    {
        joypadEntity = gameContext.GetGroup(GameMatcher.Joypad).GetSingleEntity();

        var playerGroup = gameContext.GetGroup(GameMatcher.Player);
        playerGroup.OnEntityAdded += OnPlayerCreated;
        playerGroup.OnEntityRemoved += OnPlayerDestroyed;

        triggerManager = new PlayerGunTriggerManager(joypadEntity);
    }

    private void OnPlayerDestroyed(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        playerEntity = null;
        triggerManager.playerEntity = null;
    }

    private void OnPlayerCreated(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        playerEntity = entity;
        triggerManager.playerEntity = playerEntity;
    }

    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        entitiesCleanupRegister.AddRange(entities);

        //assumption: shooting is complementing navigation
        //specificically shooting is triggered by second touch point wheras navigation by first
        //so if navigation is disabled, nothing to consider
        if(!joypadEntity.joypad.enabled)
        {
            triggerManager.Disable();
            return;
        }

        triggerManager.ManageState(entities);
    }

    protected override bool Filter(InputEntity entity)
    {
        return playerEntity!=null && playerEntity.isEnabled;
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector<InputEntity>(InputMatcher.Touches.Added());
    }

    //due to volume of input entities this system deals with his own garbage, rather than marking it for cleanup
    public void Cleanup()
    {
        foreach (var entity in entitiesCleanupRegister)
        {
            entity.Destroy();
        }

        entitiesCleanupRegister.Clear();
    }
}

