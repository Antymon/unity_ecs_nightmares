using UnityEngine;
using Entitas;
using UnityEngine.AI;


public class AIMovementBehaviour : MonoBehaviour, IEntityDeserializer, IMovementDirectionChangedListener
{
    //ToDo extract class
    const string WALKING_ANIMATION_LABEL = "IsWalking";

    private Animator playerAnimation;
    private NavMeshAgent navAgent;

    private GameEntity selfGameEntity;

    private Vector3 previousPosition;

    public void DeserializeEnitity(GameEntity entity)
    {
        this.selfGameEntity = entity;
        selfGameEntity.AddMovementDirectionChangedListener(this);
        selfGameEntity.AddPosition(transform.position);
    }

    public void OnMovementDirectionChanged(Vector3 destination)
    {
        navAgent.SetDestination(destination);
    }

    public void OnOrientationChanged(Vector3 destination)
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
        if (Vector3.Distance(previousPosition,transform.position)<.01f) //precision check
        {
            playerAnimation.SetBool(WALKING_ANIMATION_LABEL, false);
        }
        else
        {
            playerAnimation.SetBool(WALKING_ANIMATION_LABEL, true);
        }

        previousPosition = transform.position;
    }

    void LateUpdate()
    {
        selfGameEntity.ReplacePosition(transform.position);
    }
}

