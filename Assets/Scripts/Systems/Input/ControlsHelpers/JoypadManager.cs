using UnityEngine;
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
