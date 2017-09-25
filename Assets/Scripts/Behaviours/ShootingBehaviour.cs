using UnityEngine;
using Entitas;


public class ShootingBehaviour : MonoBehaviour, IEntityDeserializer, IShootListener
{
    public int cooldownTicks;

    public void DeserializeEnitity(GameEntity entity)
    {
        entity.AddGun(cooldownTicks, newCurrentHeat: 0, newShootListener: this, newTriggerDown: false);
    }

    public void OnShoot(GameEntity bullet)
    {

    }
}

