using Entitas;
using UnityEngine;

public class JoypadSystem : ReactiveSystem<InputEntity>, IInitializeSystem
{
    private IEntityDeserializer entityDeserializer;
    private GameContext gameContext;
    private GameEntity joypadEntity;

    public JoypadSystem(InputContext context, GameContext gameContext, IEntityDeserializer deserializer)
        : base(context)
    {
        this.entityDeserializer = deserializer;
        this.gameContext = gameContext;
    }

    public void Initialize()
    {
        joypadEntity = gameContext.CreateEntity();
        joypadEntity.AddEntityBinding(EntityPrefabNameBinding.JOYPAD_BINDING);
        joypadEntity.AddJoystick(false,-1);
        entityDeserializer.DeserializeEnitity(joypadEntity);
    }

    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        int joypadTouchId = joypadEntity.joystick.touchId;
        bool joypadEnabled = joypadEntity.joystick.enabled;

        foreach (var entity in entities)
        {
            if (joypadEnabled) //joypad is visible so valid actions are move or disable
            {
                foreach (var touch in entity.touches.touches)
                {
                    if(touch.fingerId == joypadTouchId)
                    {
                        if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            HideJoypad();
                        }
                        else if (touch.phase == TouchPhase.Moved)
                        {
                            MoveJoypad(touch.position);
                        }

                        break;
                    }
                }
            }
            else
            {
                foreach (var touch in entity.touches.touches)
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
        Debug.Log("ShowJoypad");

        joypadEntity.ReplacePosition(touch.position);
        joypadEntity.ReplaceJoystick(true, touch.fingerId);

        
    }

    private void MoveJoypad(Vector2 vector2)
    {
        Debug.Log("JoypadMoved");

        


    }

    private void HideJoypad()
    {
        Debug.Log("HideJoypad");

        joypadEntity.ReplaceJoystick(false, -1);
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

