using UnityEngine;
using Entitas;


public class PlayerMovmentBehaviour : MonoBehaviour, IEntityDeserializer, IMovementDirectionChangedListener
{
    public float speed = 6f;            // The speed that the player will move at.

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Animator anim;                      // Reference to the animator component.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.

    //used for animation toggling
    int framesCountSinceLastMovement = 0;
    const int MAX_FRAMES_SINCE_LAST_MOVEMENT = 5;

    public void DeserializeEnitity(GameEntity entity)
    {
        entity.AddMovementDirectionChangedListener(this) ;
    }

    public void OnMovementDirectionChanged(Vector2 direction)
    {
        Move(direction.x, direction.y);
        
        framesCountSinceLastMovement = 0;
        anim.SetBool("IsWalking", true);
    }

    public void OnOrientationChanged(Vector2 direction)
    {
        Turn(direction);
        
    }

    void Awake()
    {
        // Set up references.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (framesCountSinceLastMovement < MAX_FRAMES_SINCE_LAST_MOVEMENT)
        {
            framesCountSinceLastMovement++;
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);
    }


    void Turn(Vector2 direction)
    {
        // Create a vector from the player to the point on the floor the raycast from the mouse hit.
        Vector3 playerToMouse = Vector3.zero;
        playerToMouse.x = direction.x;
        playerToMouse.z = direction.y;

        if (playerToMouse.Equals(Vector3.zero)) return;

        // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
        Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

        // Set the player's rotation to this new rotation.
        playerRigidbody.MoveRotation(newRotatation);

    }
}

