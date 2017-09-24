﻿using UnityEngine;
using Entitas;


public class JoypadBehaviour : BindingEntitasBehaviour, IEntityDeserializer, IJoypadMovedListener
{
    override public void DeserializeEnitity(GameEntity entity)
    {
        base.DeserializeEnitity(entity);
        entity.ReplacePosition(transform.position);
        entity.AddJoypadBinding(0, this);

        entity.OnComponentReplaced += OnComponentReplaced;
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

    public void JoypadMoved(Vector2 joypadDirection)
    {
        float joypadAngle = JoypadSystem.GetAngleFromDirection(joypadDirection);
    }
}

