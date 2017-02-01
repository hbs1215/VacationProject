using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;            // The speed that the player will move at.

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Animator anim;                      // Reference to the animator component.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    //float camRayLength = 100f;          // The length of the ray from the camera into the scene.

    float m_TurnSpeed = 360f;

    void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");

        // Set up references.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        // Store the input axes.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Move the player around the scene.
        Move(h, v);

        // Turn the player to face the mouse cursor.
        Turning(h,v);

        // Animate the player.
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.fixedDeltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);

        transform.position += movement;

        Debug.Log("Mox: " + movement.x + " Moz: " + movement.z);
        Debug.Log("x: " + transform.position.x + " z: " + transform.position.z);
        //Debug.Log("Px: " + playerRigidbody.position.x + " Pz: " + playerRigidbody.position.z);
    }

    void Turning(float h, float v)
    {
        //float temp = playerRigidbody.rotation.y;

        //turnResult = playerRigidbody.rotation;

        //tempV.Set(0f, 10f, 0f);

        float turn = 0f;
        float turnResult = 0f;
        Quaternion newRotation = playerRigidbody.rotation;


        if (h > 0)
        {
            turn = h * m_TurnSpeed * Time.deltaTime;

            if(playerRigidbody.rotation.eulerAngles.y >= 270f || playerRigidbody.rotation.eulerAngles.y < 90f)
                newRotation = playerRigidbody.rotation * Quaternion.Euler(0f, turn, 0f);
            else if(playerRigidbody.rotation.eulerAngles.y > 90f && playerRigidbody.rotation.eulerAngles.y < 270f)
                newRotation = playerRigidbody.rotation * Quaternion.Euler(0f, -1 * turn, 0f);
            turnResult = newRotation.eulerAngles.y;
        }
        else if(h < 0)
        {
            turn = h * m_TurnSpeed * Time.deltaTime;

            if (playerRigidbody.rotation.eulerAngles.y > 270f || playerRigidbody.rotation.eulerAngles.y <= 90f)
                newRotation = playerRigidbody.rotation * Quaternion.Euler(0f, turn, 0f);
            else if (playerRigidbody.rotation.eulerAngles.y > 90f && playerRigidbody.rotation.eulerAngles.y < 270f)
                newRotation = playerRigidbody.rotation * Quaternion.Euler(0f, -1 * turn, 0f);
            turnResult = newRotation.eulerAngles.y;
        }
        else if (v > 0)
        {
            turn = v * m_TurnSpeed * Time.deltaTime;

            if (playerRigidbody.rotation.eulerAngles.y >= 180f)
                newRotation = playerRigidbody.rotation * Quaternion.Euler(0f, turn, 0f);
            else if (playerRigidbody.rotation.eulerAngles.y > 0f && playerRigidbody.rotation.eulerAngles.y < 180f)
                newRotation = playerRigidbody.rotation * Quaternion.Euler(0f, -1 * turn, 0f);
            turnResult = newRotation.eulerAngles.y;
        }
        else if (v < 0)
        {
            turn = v * m_TurnSpeed * Time.deltaTime;

            if (playerRigidbody.rotation.eulerAngles.y > 180f)
                newRotation = playerRigidbody.rotation * Quaternion.Euler(0f, turn, 0f);
            else if (playerRigidbody.rotation.eulerAngles.y >= 0f && playerRigidbody.rotation.eulerAngles.y < 180f)
                newRotation = playerRigidbody.rotation * Quaternion.Euler(0f, -1 * turn, 0f);
            turnResult = newRotation.eulerAngles.y;
        }



        playerRigidbody.MoveRotation(newRotation);
    }

    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool Running = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsRunning", Running);
    }
}