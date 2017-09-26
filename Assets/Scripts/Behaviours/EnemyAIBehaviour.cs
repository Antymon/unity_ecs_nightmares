using UnityEngine;
using Entitas;
using UnityEngine.AI;


public class EnemyAIBehaviour : MonoBehaviour, IEntityDeserializer
{
    public int range = 100;

    public Transform threat;
    public Transform[] shelters;

    private Ray shootRay = new Ray();
    private RaycastHit shootHit;
    private int shootableMask;

    private Transform currentSafePoint;
    private NavMeshAgent navAgent;

    private GameEntity selfGameEntity;

    bool pursuePlayer = false;

    public void DeserializeEnitity(GameEntity entity)
    {
        this.selfGameEntity = entity;
    }

    private float GetNormalizedHealth()
    {
        return (float)selfGameEntity.health.healthPoints / selfGameEntity.health.healthPointsCap;
    }

    public void Start()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        if(GetNormalizedHealth()<.5f)
        {
            if (selfGameEntity.gun.triggerDown)
            {
                selfGameEntity.gun.triggerDown = false;
            }
            SeekSafety();
        }
        else
        {
            if (threat.gameObject.activeInHierarchy)
            {
                Attack();
            }
            else
            {
                selfGameEntity.gun.triggerDown = false;
            }
        }
    }

    private void Attack()
    {
        var distance = threat.position - transform.position;

        if(distance.sqrMagnitude > range*range) //ToDo: quicker to multiply, but can be premul
        {
            //ToDo:potentially expensive to call this on every frame
            navAgent.SetDestination(threat.position);
            if (selfGameEntity.gun.triggerDown)
            {
                selfGameEntity.gun.triggerDown = false;
            }
        }
        else
        {
            if (navAgent.hasPath)
            {
                navAgent.ResetPath();
            }

            transform.forward = distance;
            if (!selfGameEntity.gun.triggerDown)
            {
                selfGameEntity.gun.triggerDown = true;
            }
        }
    }

    private void SeekSafety()
    {
        if (currentSafePoint == null)
        {
            SetShelterMaximizingReachingChance();

            if (currentSafePoint != null)
            {
                navAgent.SetDestination(currentSafePoint.position);
            }
        }
        else
        {
            if (!IsSafe(currentSafePoint))
            {
                navAgent.SetDestination(transform.position);
                currentSafePoint = null;
            }
        }
    }

    //maximizing value of shelter choice
    private void SetShelterMaximizingReachingChance()
    {
        float bestSafetyValue = 0;

        foreach (var shelter in shelters)
        {
            if (IsSafe(shelter))
            {
                float safetyValue = GetReachingSafelyChance(shelter);

                if (safetyValue > bestSafetyValue)
                {
                    bestSafetyValue = safetyValue;
                    currentSafePoint = shelter;
                }
            }
        }
    }


    //heuristics
    private float GetReachingSafelyChance(Transform shelterTransform)
    {
        Vector3 threatDistance = threat.position - transform.position;
        Vector3 safePointDistance = shelterTransform.position - transform.position;

        return Mathf.Abs(Vector3.Angle(threatDistance, safePointDistance));
    }

    private bool IsSafe(Transform shelterTransform)
    {
        shootRay.origin = threat.position;
        Vector3 direction = shelterTransform.position - threat.position;
        direction.y = 0f; //we are considering  floor XZ surface only
        shootRay.direction = direction;

        float distance = direction.magnitude;

        //spot is out of range at the moment thus safe
        if(distance>range)
        {
            return true;
        }

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if(Physics.Raycast(shootRay, out shootHit, distance, shootableMask))
        {
            //some obstacle would be hit, but not me, thus making it safe
            return shootHit.collider.transform != this.transform;
        }

        //spot is within clean shot
        return false;
    }
}

