using Entitas;
using UnityEngine;

public class TriggerBulletSystem : ReactiveSystem<InputEntity>, IInitializeSystem
{
    private GameContext gameContext;
    private GameEntity joypadEntity;

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

            entity.Destroy();
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
}

