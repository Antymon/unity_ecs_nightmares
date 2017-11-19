using Entitas;
using System.Collections.Generic;
using UnityEngine;

public interface ITouchDrivenStateManager
{
    void ManageState(List<InputEntity> entities);
    void Disable();
    bool IsEnabled();
}

//template method manager that enables or disables injected item based on:
//touch input
//api calls
public abstract class TouchDrivenStateManager : ITouchDrivenStateManager
{
    public void ManageState(List<InputEntity> touchEntities)
    {
        foreach (var entity in touchEntities)
        {
            int touchId = GetTouchId();

            var touches = entity.touches.touches;

            bool touchFound = false;

            if (IsEnabled())
            {
                foreach (var touch in touches)
                {
                    if (touch.fingerId == touchId)
                    {
                        touchFound = true;

                        OnTouchFound(touch);

                        break;
                    }
                }

                if (!touchFound)
                {
                    Disable();
                }
            }
            else //disabled
            {
                foreach (var touch in touches)
                {
                    if (ShouldEnable(touch))
                    {
                        Enable(touch);
                        break;
                    }
                }
            }
        }
    }

    public abstract void Disable();


    protected abstract void Enable(Touch touch);

    public abstract bool IsEnabled();

    protected abstract bool ShouldEnable(Touch touch);


    protected abstract int GetTouchId();

    protected abstract void OnTouchFound(Touch touch);
}

//implementation of TouchDrivenStateManager
//that manages joypad via its entity state
public class JoypadManager : TouchDrivenStateManager
{
    public GameEntity playerEnity;

    private GameEntity joypadEntity;

    public JoypadManager(GameEntity joypadEntityToManage)
    {
        this.joypadEntity = joypadEntityToManage;
    }

    protected override bool ShouldEnable(Touch touch)
    {
        return touch.phase == TouchPhase.Began;
    }

    protected override int GetTouchId()
    {
        return joypadEntity.joypad.touchId;
    }

    public override bool IsEnabled()
    {
        //joypad is visible so valid actions are move or disable
        return joypadEntity.joypad.enabled;
    }

    public override void Disable()
    {
        joypadEntity.ReplaceJoypad(newEnabled: false, newTouchId: -1);
    }

    protected override void Enable(Touch touch)
    {
        joypadEntity.ReplacePosition(touch.position);
        joypadEntity.ReplaceJoypad(newEnabled: true, newTouchId: touch.fingerId);
    }
    protected override void OnTouchFound(Touch touch)
    {
        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            Disable();
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            MoveJoypad(touch.position);
        }
    }

    private void MoveJoypad(Vector2 touchPosition)
    {
        Vector2 joypadDirection = touchPosition - (Vector2)joypadEntity.position.position;

        joypadEntity.joypadBinding.listener.OnJoypadMoved(joypadDirection);

        //it's hard to aim while running, this gives ability to rotate without changing position
        bool onlyRotationAffected = joypadDirection.magnitude < joypadEntity.joypadBinding.radius;

        playerEnity.ReplaceMovementDirection(joypadDirection, onlyRotationAffected);
    }
}

public class PlayerGunTriggerManager : TouchDrivenStateManager
{
    public GameEntity playerEntity;

    private GameEntity joypadEntity;

    private int triggerTouchId = -1;

    public PlayerGunTriggerManager(GameEntity joypadEntityToManage)
    {
        this.joypadEntity = joypadEntityToManage;
    }

    public override void Disable()
    {
        if (playerEntity.gun.triggerDown)
        {
            playerEntity.gun.triggerDown = false;
            triggerTouchId = -1;
        }
    }

    protected override void Enable(Touch touch)
    {
        triggerTouchId = touch.fingerId;
        playerEntity.gun.triggerDown = true;
    }

    public override bool IsEnabled()
    {
        return playerEntity.gun.triggerDown;
    }

    protected override bool ShouldEnable(Touch touch)
    {
        return touch.fingerId != joypadEntity.joypad.touchId && touch.phase == TouchPhase.Began;
    }

    protected override int GetTouchId()
    {
        return triggerTouchId;
    }

    protected override void OnTouchFound(Touch touch)
    {
        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            Disable();
        }
    }
}