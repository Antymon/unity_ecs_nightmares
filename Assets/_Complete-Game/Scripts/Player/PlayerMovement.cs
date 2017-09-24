﻿using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace CompleteProject
{
    public class PlayerMovement : MonoBehaviour
    {
        public NavigationCircleManager navigationCircle;

        public float speed = 6f;            // The speed that the player will move at.


        Vector3 movement;                   // The vector to store the direction of the player's movement.
        Animator anim;                      // Reference to the animator component.
        Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
#if !MOBILE_INPUT
        int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        float camRayLength = 100f;          // The length of the ray from the camera into the scene.
#endif

        void Awake ()
        {
#if !MOBILE_INPUT
            // Create a layer mask for the floor layer.
            floorMask = LayerMask.GetMask ("Floor");
#endif

            // Set up references.
            anim = GetComponent <Animator> ();
            playerRigidbody = GetComponent <Rigidbody> ();
        }


        void FixedUpdate ()
        {
            // Store the input axes.
            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

            // Move the player around the scene.

            if (navigationCircle.IsPointDraggedAndInsideOfACircle())
            {
                Move(navigationCircle.lastNavigationTouchPoint.x, navigationCircle.lastNavigationTouchPoint.y);
            }

            // Turn the player to face the mouse cursor.
            Turning ();

            // Animate the player.
            Animating (h, v);
        }


        void Move (float h, float v)
        {
            // Set the movement vector based on the axis input.
            movement.Set (h, 0f, v);
            
            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement.normalized * speed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            playerRigidbody.MovePosition (transform.position + movement);
        }


        void Turning ()
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = Vector3.zero;
            playerToMouse.x = navigationCircle.lastNavigationTouchPoint.x;
            playerToMouse.z = navigationCircle.lastNavigationTouchPoint.y;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotatation = Quaternion.LookRotation (playerToMouse);

            // Set the player's rotation to this new rotation.
            playerRigidbody.MoveRotation (newRotatation);

        }


        void Animating (float h, float v)
        {
            // Create a boolean that is true if either of the input axes is non-zero.
            bool walking = h != 0f || v != 0f;

            // Tell the animator whether or not the player is walking.
            anim.SetBool ("IsWalking", walking);
        }
    }
}