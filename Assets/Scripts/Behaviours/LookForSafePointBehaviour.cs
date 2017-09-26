using UnityEngine;
using Entitas;
using UnityEngine.AI;


public class LookForSafePointBehaviour : MonoBehaviour, IEntityDeserializer
{
    public int range = 100;

    public Transform threat;
    public Transform[] shelters;

    private Ray shootRay = new Ray();
    private RaycastHit shootHit;
    private int shootableMask;

    private Transform currentSafePoint;
    private NavMeshAgent navAgent;


    public void DeserializeEnitity(GameEntity entity)
    {
        //entity.ReplaceHealth(healthPoints, healthPointsCap);
    }

    public void Start()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        if(currentSafePoint==null)
        {
            SetShelterMaximizingReachingChance();

            if(currentSafePoint!=null)
            {
                navAgent.SetDestination(currentSafePoint.position);
            }
        }
        else
        {
            if(!IsSafe(currentSafePoint))
            {
                navAgent.SetDestination(transform.position);
                currentSafePoint = null;
            }
        }

    }

    private void SetShelterMaximizingReachingChance()
    {
        float bestSafetyValue = 0;

        foreach (var shelter in shelters)
        {
            if (IsSafe(shelter))
            {
                float safetyValue = GetReachingSafetyChance(shelter);

                if (safetyValue > bestSafetyValue)
                {
                    bestSafetyValue = safetyValue;
                    currentSafePoint = shelter;
                }
            }
        }
    }


    //heuristics
    private float GetReachingSafetyChance(Transform shelterTransform)
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

