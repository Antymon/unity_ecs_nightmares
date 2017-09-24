using UnityEngine;
using Entitas;


public class DamageBehaviour : MonoBehaviour, IEntityDeserializer
{
    public int healthPointsDamaged;

    public void DeserializeEnitity(GameEntity entity)
    {
        entity.ReplaceDamage(healthPointsDamaged);
    }
}

