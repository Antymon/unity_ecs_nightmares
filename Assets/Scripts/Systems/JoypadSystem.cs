using Entitas;
using UnityEngine;

public class JoypadSystem : ReactiveSystem<InputEntity>, IInitializeSystem
{
    //stateless utility
    public static float GetAngleFromDirection(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    }

    private IEntityDeserializer entityDeserializer;
    private GameContext gameContext;
    private GameEntity joypadEntity;
    private GameEntity playerEnity;

    public JoypadSystem(InputContext context, GameContext gameContext, IEntityDeserializer deserializer)
        : base(context)
    {
        this.entityDeserializer = deserializer;
        this.gameContext = gameContext;
    }

    //create joypad binding, fetch player entity
    public void Initialize()
    {
        joypadEntity = gameContext.CreateEntity();
        joypadEntity.AddEntityBinding(EntityPrefabNameBinding.JOYPAD_BINDING);
        joypadEntity.AddJoystick(newEnabled: false, newTouchId:-1);
        entityDeserializer.DeserializeEnitity(joypadEntity);

        playerEnity = gameContext.GetGroup(GameMatcher.Player).GetSingleEntity();
    }

    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        int joypadTouchId = joypadEntity.joystick.touchId;
        bool joypadEnabled = joypadEntity.joystick.enabled;

        foreach (var entity in entities)
        {
            var touches = entity.touches.touches;

            if (joypadEnabled) //joypad is visible so valid actions are move or disable
            {
                foreach (var touch in touches)
                {
                    if(touch.fingerId == joypadTouchId)
                    {
                        if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            HideJoypad();
                        }
                        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                        {
                            MoveJoypad(touch.position);
                        }

                        break;
                    }
                }
            }
            else
            {
                foreach (var touch in touches)
                {
                    if(touch.phase == TouchPhase.Began)
                    {
                        ShowJoypad(touch);
                        
                        break;
                    }
                }
            }
            entity.Destroy();
        }
    }

    private void ShowJoypad(Touch touch)
    {
        joypadEntity.ReplacePosition(touch.position);
        joypadEntity.ReplaceJoystick(newEnabled:true,newTouchId:touch.fingerId);
    }

    private void MoveJoypad(Vector2 touchPosition)
    {
        Vector2 joypadDirection = touchPosition - (Vector2)joypadEntity.position.position;

        joypadEntity.joypadBinding.listener.OnJoypadMoved(joypadDirection);

        //it's hard to aim while running, this gives ability to rotate without changing position
        bool onlyRotationAffected = joypadDirection.magnitude < joypadEntity.joypadBinding.radius;

        playerEnity.ReplaceMovementDirection(joypadDirection,onlyRotationAffected);
    }

    private void HideJoypad()
    {
        joypadEntity.ReplaceJoystick(newEnabled: false, newTouchId:-1);
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

