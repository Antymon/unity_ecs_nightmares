﻿using UnityEngine;
using Entitas;


public class ShootingBehaviour : MonoBehaviour, IEntityDeserializer, IShootListener
{
    public int cooldownTicks; //effectively frames between shots
    public int range; //raycast range
    public int damagePerShot;

    public float missError = 0f; //max shooting error

    public Transform barrelEnd;

    private ShootingVisualEffects shootingEffects;

    private Ray shootRay = new Ray();                       // A ray from the gun end forwards.
    private RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    private int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.

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

    void Awake()
    {
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");
        shootingEffects = GetComponentInChildren<ShootingVisualEffects>();
    }

    public void OnShoot(GameEntity bullet)
    {
        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = barrelEnd.position;

        //adding some randomness in order to avoid perfect shooting 
        //Unity random can be used here; only Entitas' randomness is controlled
        var direction = barrelEnd.forward;
        direction.x += (Random.value - .5f) * missError;
        direction.z += (Random.value - .5f) * missError;

        shootRay.direction = direction;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            var entitasBinding = shootHit.collider.GetComponent<IEntitasBinding>();

            if(entitasBinding!=null)
            {
                var coll = Contexts.sharedInstance.input.CreateEntity();
                coll.AddCollision(bullet, entitasBinding.GetEntity());
            }
            else
            {
                bullet.Destroy();
            }

            shootingEffects.Shoot(shootHit.point);
        }
        // If the raycast didn't hit anything on the shootable layer...
        else
        {
            bullet.Destroy();
            //the fullest extent of the gun's range.
            var rayEndPoint = shootRay.origin + shootRay.direction * range;

            shootingEffects.Shoot(rayEndPoint);
        }
    }
}

