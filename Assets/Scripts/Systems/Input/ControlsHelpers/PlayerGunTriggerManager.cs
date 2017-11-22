using UnityEngine;

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