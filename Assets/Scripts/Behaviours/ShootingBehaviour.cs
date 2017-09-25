using UnityEngine;
using Entitas;


public class ShootingBehaviour : MonoBehaviour, IEntityDeserializer, IShootListener
{
    public int cooldownTicks;
    public int range;
    public int damagePerShot;

    public Transform barrelEnd;

    private CompleteProject.ShootingEffects shootingEffects;

    public void DeserializeEnitity(GameEntity entity)
    {
        entity.AddGun(
            cooldownTicks, 
            newCurrentHeat: 0, 
            newShootListener: this, 
            newTriggerDown: false,
            newRange: range,
            newDamagePerShot: damagePerShot
            );
    }

    Ray shootRay = new Ray();                       // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.

    void Awake()
    {
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");
        shootingEffects = GetComponentInChildren<CompleteProject.ShootingEffects>();
        shootingEffects.effectsLastTicks = cooldownTicks;
    }

    public void OnShoot(GameEntity bullet)
    {
        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = barrelEnd.position;
        shootRay.direction = barrelEnd.forward;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            //ToDo: add collision

            //var coll = Contexts.sharedInstance.input.CreateEntity();
            //coll.AddCollision(bullet,)

            shootingEffects.Shoot(shootHit.point);
        }
        // If the raycast didn't hit anything on the shootable layer...
        else
        {
            //the fullest extent of the gun's range.
            var rayEndPoint = shootRay.origin + shootRay.direction * range;

            shootingEffects.Shoot(rayEndPoint);
        }
    }
}

