using UnityEngine;
using Entitas;


public class HealthBehaviour : MonoBehaviour, IEntityDeserializer
{
    public int healthPoints;
    public int healthPointsCap;

    public void DeserializeEnitity(GameEntity entity)
    {
        entity.ReplaceHealth(healthPoints, healthPointsCap);
    }
}

