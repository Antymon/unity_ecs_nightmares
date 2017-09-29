using UnityEngine;
using Entitas;
using UnityEngine.AI;


public class AIMovementBehaviour : MonoBehaviour, IEntityDeserializer, IMovementDestinationChangedListener
{
    //ToDo: extract class
    const string WALKING_ANIMATION_LABEL = "IsWalking";

    private Animator playerAnimation;
    private NavMeshAgent navAgent;

    private GameEntity selfGameEntity;

    public void DeserializeEnitity(GameEntity entity)
    {
        this.selfGameEntity = entity;
        selfGameEntity.AddMovementDestinationChangedListener(this);
        selfGameEntity.AddPosition(transform.position);
    }

    public void OnMovementDestinationChanged(Vector3 destination)
    {
        navAgent.SetDestination(destination);
    }

    public void OnOrientationDestinationChanged(Vector3 destination)
    {
        if (destination.Equals(transform.position))
        {
            return;
        }
        transform.forward = destination-transform.position; 
    }

    void Awake()
    {
        playerAnimation = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        playerAnimation.SetBool(WALKING_ANIMATION_LABEL, selfGameEntity.positionChanged.ticksStationary==0);
    }

    void LateUpdate()
    {
        selfGameEntity.ReplacePosition(transform.position);
    }
}

