using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBulletSystem : ReactiveSystem<InputEntity>, IInitializeSystem, ICleanupSystem
{
    private GameContext gameContext;
    private GameEntity joypadEntity;

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
        joypadEntity = gameContext.GetGroup(GameMatcher.Joystick).GetSingleEntity();
    }

    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        entitiesCleanupRegister.AddRange(entities);

        //assumption: shooting is complementing navigation
        //specificically shooting is triggered by second touch point wheras navigation by first
        //so if navigation is disabled, nothing to consider
        if(!joypadEntity.joystick.enabled)
        {
            return;
        }

        int joypadTakenTouchId = joypadEntity.joystick.touchId;

        foreach (var entity in entities)
        {
            var touches = entity.touches.touches;

            foreach (var touch in touches)
            {
                if (touch.fingerId != joypadTakenTouchId)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        TriggerBullet();
                    }
                }
            }

            //entity.Destroy();
        }
    }

    private void TriggerBullet()
    {
        Debug.Log("trigger bullet");
    }

    protected override bool Filter(InputEntity entity)
    {
        return true;
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector<InputEntity>(InputMatcher.Touches.Added());
    }

    public void Cleanup()
    {
        foreach (var entity in entitiesCleanupRegister)
        {
            entity.Destroy();
        }

        entitiesCleanupRegister.Clear();
    }
}

