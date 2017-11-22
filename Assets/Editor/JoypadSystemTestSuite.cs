using Entitas;
using NUnit.Framework;
using UnityEngine;

public class JoypadSystemTestSuite : EntitasTestSuite, IJoypadMovedListener
{
    Vector2 joypadDirection;

    private Touch CreateFakeTouch(TouchPhase phase, int id, Vector2 mousePosition)
    {
        Touch touch = new Touch();
        touch.phase = phase;
        touch.position = mousePosition;
        touch.fingerId = id;

        return touch;
    }

    private void NotifyTouches(Touch[] touches)
    {
        if (!gameContext.isPause)
        {
            CreateInputEntity().AddTouches(touches);
        }
    }

    private GameEntity SimulateJoypadConditions(bool touch, bool player)
    {
        var joypadEntity = gameContext.GetGroup(GameMatcher.Joypad).GetSingleEntity();

        //normally added on Unity's side
        joypadEntity.AddJoypadBinding(newRadius: 50, newListener: this);


        if (player)
        {
            var playerEntity = CreateGameEntity();
            playerEntity.isPlayer = true;
            playerEntity.AddGun(1, 1, null, false, 100, 1);
        }

        if (touch)
        {
            Touch[] touches = new Touch[] { CreateFakeTouch(TouchPhase.Began, 1, Vector2.zero) };

            NotifyTouches(touches);
        }

        UpdateSystems();

        return joypadEntity;
    }

    [Test]
    public void JoypadDisabled_OnPlayerAndTouchAppears_JoypadEnabled()
    {
        var joypadEntity = SimulateJoypadConditions(true, true);

        Assert.IsTrue(joypadEntity.joypad.enabled);
    }

    [Test]
    public void JoypadEnabled_NoTouchEventsButPlayerExists_JoypadDisabled()
    {
        var joypadEntity = SimulateJoypadConditions(false, true);

        Assert.IsFalse(joypadEntity.joypad.enabled);
    }

    [Test]
    public void JoypadEnabled_TouchEventsButNoPlayerExists_JoypadDisabled()
    {
        var joypadEntity = SimulateJoypadConditions(true, false);

        Assert.IsFalse(joypadEntity.joypad.enabled);
    }

    [Test]
    public void JoypadEnabled_OnTouchMoved_JoypadDirectionChanges()
    {
        var joypadEntity = SimulateJoypadConditions(true, true);

        Assert.AreEqual(Vector3.zero, joypadEntity.position.position);

        var touch = CreateFakeTouch(TouchPhase.Moved, 1, Vector2.one);

        NotifyTouches(new Touch[] { touch });

        UpdateSystems();

        Assert.IsTrue(joypadEntity.joypad.enabled);

        //direction should be subtraction of current touch and position
        Assert.AreEqual(Vector2.one, joypadDirection);
    }

    [Test]
    public void JoypadEnabled_MoreTouches_JoypadKeepsOriginalTouchId()
    {
        var joypadEntity = SimulateJoypadConditions(true, true);

        Assert.IsTrue(joypadEntity.joypad.enabled);

        var touch1 = CreateFakeTouch(TouchPhase.Moved, 1, Vector2.one);
        var touch2 = CreateFakeTouch(TouchPhase.Began, 2, Vector2.one);

        NotifyTouches(new Touch[] { touch2, touch1 });

        UpdateSystems();

        touch2.phase = TouchPhase.Moved;

        NotifyTouches(new Touch[] { touch2, touch1 });

        UpdateSystems();

        Assert.IsTrue(joypadEntity.joypad.enabled);
        Assert.AreEqual(1, joypadEntity.joypad.touchId);

        //original touch disappears

        NotifyTouches(new Touch[] { touch2 });

        UpdateSystems();

        Assert.IsFalse(joypadEntity.joypad.enabled);
    }

    public void OnJoypadMoved(Vector2 direction)
    {
        joypadDirection = direction;
    }

    [Test]
    public void JoypadEnabled_MoreThan1Touch_TriggerEnabled()
    {
        SimulateJoypadConditions(true, true);
        var playerEntity = gameContext.GetGroup(GameMatcher.Player).GetSingleEntity();

        var touch1 = CreateFakeTouch(TouchPhase.Moved, 1, Vector2.one);
        var touch2 = CreateFakeTouch(TouchPhase.Began, 2, Vector2.one);

        NotifyTouches(new Touch[] { touch2, touch1 });

        UpdateSystems();

        Assert.IsTrue(playerEntity.gun.triggerDown);
    }

    [Test]
    public void JoypadDisabled_TriggerDisabled()
    {
        SimulateJoypadConditions(true, true);
        var playerEntity = gameContext.GetGroup(GameMatcher.Player).GetSingleEntity();

        var touch1 = CreateFakeTouch(TouchPhase.Moved, 1, Vector2.one);
        var touch2 = CreateFakeTouch(TouchPhase.Began, 2, Vector2.one);

        NotifyTouches(new Touch[] { touch2, touch1 });

        UpdateSystems();

        Assert.IsTrue(playerEntity.gun.triggerDown);

        //joypad touch is gone, and no begin touches so joypad will disable
        var touch3 = CreateFakeTouch(TouchPhase.Moved, 3, Vector2.one);
        touch2.phase = TouchPhase.Moved;
        NotifyTouches(new Touch[] { touch2, touch3 });

        UpdateSystems();

        Assert.IsFalse(playerEntity.gun.triggerDown);
    }

    [Test]
    public void OneTouch_TriggerDisabled()
    {
        var joypadEntity = SimulateJoypadConditions(true, true);
        var playerEntity = gameContext.GetGroup(GameMatcher.Player).GetSingleEntity();

        Assert.IsTrue(joypadEntity.joypad.enabled);
        Assert.IsFalse(playerEntity.gun.triggerDown);
    }

    [Test]
    public void TouchFallsToOne_TriggerDisables()
    {
        SimulateJoypadConditions(true, true);
        var playerEntity = gameContext.GetGroup(GameMatcher.Player).GetSingleEntity();

        var touch1 = CreateFakeTouch(TouchPhase.Moved, 1, Vector2.one);
        var touch2 = CreateFakeTouch(TouchPhase.Began, 2, Vector2.one);

        NotifyTouches(new Touch[] { touch2, touch1 });

        UpdateSystems();

        Assert.IsTrue(playerEntity.gun.triggerDown);

        NotifyTouches(new Touch[] { touch1 });

        UpdateSystems();

        Assert.IsFalse(playerEntity.gun.triggerDown);
    }
}

