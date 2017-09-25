using UnityEngine;
using Entitas;


public class MovementBehaviour : MonoBehaviour, IEntityDeserializer, IMovementDirectionChangedListener
{
    const string WALKING_ANIMATION_LABEL = "IsWalking";

    //used for animation toggling
    const int MAX_FRAMES_SINCE_LAST_MOVEMENT = 5;
    int framesCountSinceLastMovement = 0;

    public float speed = 6f;            // The speed that the player will move at.

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Animator playerAnimation;                      // Reference to the animator component.
    Rigidbody agentRigidbody;

    public void DeserializeEnitity(GameEntity entity)
    {
        entity.AddMovementDirectionChangedListener(this) ;
    }

    public void OnMovementDirectionChanged(Vector2 direction)
    {
        Move(direction.x, direction.y);
        
        framesCountSinceLastMovement = 0;
        playerAnimation.SetBool(WALKING_ANIMATION_LABEL, true);
    }

    public void OnOrientationChanged(Vector2 direction)
    {
        Turn(direction);  
    }

    void Awake()
    {
        playerAnimation = GetComponent<Animator>();
        agentRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (framesCountSinceLastMovement < MAX_FRAMES_SINCE_LAST_MOVEMENT)
        {
            framesCountSinceLastMovement++;
        }
        else
        {
            playerAnimation.SetBool(WALKING_ANIMATION_LABEL, false);
        }
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        agentRigidbody.MovePosition(transform.position + movement);
    }


    void Turn(Vector2 direction)
    {
        Vector3 playerToMouse = Vector3.zero;
        playerToMouse.x = direction.x;
        playerToMouse.z = direction.y;

        //avoiding precison problems
        //sqr is faster
        if (playerToMouse.sqrMagnitude < 0.01f) return;

        // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
        Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

        agentRigidbody.MoveRotation(newRotatation);

    }
}

