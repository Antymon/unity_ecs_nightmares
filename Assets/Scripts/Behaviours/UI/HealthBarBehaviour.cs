using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class HealthBarBehaviour : BindingEntitasBehaviour, IHealthBarListener
{
    public int agentId; //id that refers to id on agent entity

    public Slider healthSlider;
    public Text agentNameTextField;

    override public void DeserializeEnitity(GameEntity entity)
    {
        base.DeserializeEnitity(entity);
        entity.AddHealthBar(agentId, this);
    }

    public void OnHealthChanged(float value)
    {
        healthSlider.value = value;
    }

    public void OnNameChanged(string name)
    {
        agentNameTextField.text = name;
    }
}

