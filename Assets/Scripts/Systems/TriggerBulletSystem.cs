using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBulletSystem : ReactiveSystem<InputEntity>, IInitializeSystem, ICleanupSystem
{
    private GameContext gameContext;
    private GameEntity joypadEntity;
    private GameEntity playerEntity;

    private int triggerTouchId = -1;

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
        playerEntity = gameContext.GetGroup(GameMatcher.Player).GetSingleEntity();
    }

    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        entitiesCleanupRegister.AddRange(entities);

        //assumption: shooting is complementing navigation
        //specificically shooting is triggered by second touch point wheras navigation by first
        //so if navigation is disabled, nothing to consider
        if(!joypadEntity.joystick.enabled)
        {
            if (playerEntity.gun.triggerDown)
            {
                ReleaseTrigger();
            }
            return;
        }

        int joypadTakenTouchId = joypadEntity.joystick.touchId;

        foreach (var entity in entities)
        {
            var touches = entity.touches.touches;

            bool touchFound = false;

            if (playerEntity.gun.triggerDown)
            {
                foreach (var touch in touches)
                {
                    if (touch.fingerId == triggerTouchId)
                    {
                        touchFound = true;

                        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            ReleaseTrigger();
                        }

                        break;
                    }
                }
                
                if (!touchFound)
                {
                    ReleaseTrigger();
                }
            }
            else
            {
                foreach (var touch in touches)
                {
                    if (touch.fingerId != joypadTakenTouchId)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            PullTrigger(touch);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void PullTrigger(Touch touch)
    {
        triggerTouchId = touch.fingerId;
        playerEntity.gun.triggerDown = true;
        Debug.Log("trigger down");
    }

    private void ReleaseTrigger()
    {
        playerEntity.gun.triggerDown = false;
        triggerTouchId = -1;
        Debug.Log("trigger up");
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

