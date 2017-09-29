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

        //radius verification was meant to give ability to rotate without changing position
        //if red dot is whitin joypad rotation is possible but changing position is not
        var rectTransform = GetComponent<RectTransform>();
        outerRadius = rectTransform.rect.width / 2f;

        entity.AddJoypadBinding(newRadius: outerRadius, newListener: this);

        entity.OnComponentReplaced += OnComponentReplaced;

        gameObject.SetActive(entity.joypad.enabled);
    }

    private void OnComponentReplaced(IEntity entity, int index, IComponent previousComponent, IComponent newComponent)
    {
        JoypadComponent joystickComponent = newComponent as JoypadComponent;

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

