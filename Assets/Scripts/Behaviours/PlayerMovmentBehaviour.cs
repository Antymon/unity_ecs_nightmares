using UnityEngine;
using Entitas;


public class PlayerMovmentBehaviour : MonoBehaviour, IEntityDeserializer, IMovementDirectionChangedListener
{

    public void DeserializeEnitity(GameEntity entity)
    {
        entity.AddMovementDirectionChangedListener(this) ;
    }

    public void OnMovementDirectionChanged(Vector2 direction)
    {
        Move(direction.x, direction.y);
        Turning(direction);
        Animating(direction.x, direction.y);
    }

    public float speed = 6f;            // The speed that the player will move at.

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Animator anim;                      // Reference to the animator component.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.


    void Awake()
    {
        // Set up references.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
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


    void Turning(Vector2 direction)
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


    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
    }
}

