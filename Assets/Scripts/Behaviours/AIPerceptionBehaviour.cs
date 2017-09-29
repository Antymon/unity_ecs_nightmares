using UnityEngine;
using Entitas;
using UnityEngine.AI;
using System.Linq;


public class AIPerceptionBehaviour : MonoBehaviour, IEntityDeserializer, IPositionVerificationCallback
{
    public int attackDistance = 10;
    public float attackRecoverHealthThreshold = .5f;

    public Transform[] shelters;

    private Ray shootRay;
    private RaycastHit shootHit;
    private int shootableMask;

    private GameEntity selfGameEntity;

    public void DeserializeEnitity(GameEntity selfGameEntity)
    {
        this.selfGameEntity = selfGameEntity;

        var shelterPositions = shelters.Select(s => s.position).ToArray();

        this.selfGameEntity.AddAIPerception(shelterPositions, attackDistance, attackRecoverHealthThreshold, this);
    }

    public void Start()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        shootRay = new Ray();
    }

    public bool IsPositionSafe(Vector3 position)
    {
        return IsPositionSafe(position, selfGameEntity);
    }

    public bool IsPositionSafe(Vector3 position, GameEntity forAgent)
    {
        var self = forAgent;
        var other = forAgent.agent.target;

        shootRay.origin = other.position.position;
        Vector3 direction = position - other.position.position;
        direction.y = 0f; //we are considering  floor XZ surface only
        shootRay.direction = direction;

        float distance = direction.magnitude;

        //spot is out of range at the moment thus safe
        if (distance > other.gun.range)
        {
            return true;
        }

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, distance, shootableMask))
        {
            var agentBehaviour = shootHit.collider.gameObject.GetComponent<AgentBehaviour>();

            //some obstacle would be hit, but not me, thus making it safe
            if (agentBehaviour == null)
                return true;

            //if someone would be hit but not self, then safe
            return agentBehaviour.agentId != self.agent.id;
        }

        //spot is within clean shot
        return false;
    }


}

