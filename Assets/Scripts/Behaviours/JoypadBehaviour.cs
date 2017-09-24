using UnityEngine;
using Entitas;


public class JoypadBehaviour : BindingEntitasBehaviour, IEntityDeserializer, IJoypadMovedListener
{
    public Transform outerCircle;
    public Transform innerCircle;

    float outerRadius;

    override public void DeserializeEnitity(GameEntity entity)
    {
        base.DeserializeEnitity(entity);
        entity.ReplacePosition(transform.position);

        //ToDo: radius verification is meant as improvement: to separate rotation from actual movement
        var rectTransform = GetComponent<RectTransform>();
        outerRadius = rectTransform.rect.width / 2f;

        entity.AddJoypadBinding(newRadius: outerRadius, newListener: this);

        entity.OnComponentReplaced += OnComponentReplaced;

        gameObject.SetActive(entity.joystick.enabled);
    }

    private void OnComponentReplaced(IEntity entity, int index, IComponent previousComponent, IComponent newComponent)
    {
        JoystickComponent joystickComponent = newComponent as JoystickComponent;

        if(joystickComponent != null)
        {
            gameObject.SetActive(joystickComponent.enabled);
            transform.position = this.entity.position.position;
        }
    }

    public void OnJoypadMoved(Vector2 joypadDirection)
    {
        float joypadAngle = JoypadSystem.GetAngleFromDirection(joypadDirection);

        UpdateShape(joypadAngle, joypadDirection);
    }

    void UpdateShape(float angle, Vector2 direction)
    {
        outerCircle.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector3 stickpos = innerCircle.localPosition;
        stickpos.y = Mathf.Clamp(direction.magnitude, 0, outerRadius);
        innerCircle.localPosition = stickpos;
    }
}

