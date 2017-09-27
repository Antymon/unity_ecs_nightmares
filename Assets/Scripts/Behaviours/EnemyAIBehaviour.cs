﻿using UnityEngine;
using Entitas;
using UnityEngine.AI;
using System.Linq;


public class EnemyAIBehaviour : MonoBehaviour, IEntityDeserializer, IPositionVerificationCallback
{
    public int attackDistance = 10;
    public float attackRecoverHealthThreshold = .5f;

    public Transform[] shelters;

    private Ray shootRay;
    private RaycastHit shootHit;
    private int shootableMask;

    private GameEntity otherGameEntity;

    public void DeserializeEnitity(GameEntity selfGameEntity)
    {
        this.otherGameEntity = selfGameEntity.agent.target;

        var shelterPositions = shelters.Select(s => s.position).ToArray();

        selfGameEntity.AddAIPerception(shelterPositions, attackDistance, attackRecoverHealthThreshold, this);
    }

    public void Start()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        shootRay = new Ray();
    }

    public bool IsPositionSafe(Vector3 position)
    {
        shootRay.origin = otherGameEntity.position.position;
        Vector3 direction = position - otherGameEntity.position.position;
        direction.y = 0f; //we are considering  floor XZ surface only
        shootRay.direction = direction;

        float distance = direction.magnitude;

        //spot is out of range at the moment thus safe
        if (distance > otherGameEntity.gun.range)
        {
            return true;
        }

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, distance, shootableMask))
        {
            //some obstacle would be hit, but not me, thus making it safe
            return shootHit.collider.transform != this.transform;
        }

        //spot is within clean shot
        return false;
    }




}

