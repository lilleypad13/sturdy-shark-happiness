using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Public variables
    public float startingSpeed = 1.0f;
    public float maxSpeed = 2.0f;
    public float rotationSpeed = 2.0f;
    public float acceleration = 2.0f;

    public Light playerLight;
    public float surfacedLightSpotAngle = 30f;

    // Public static variables
    public static bool canBeSeen;

    // Private variables
    private float moveSpeed;
    private float moveVertical;
    private float moveHorizontal;
    private float inputDelay;
    private Rigidbody2D rb2d;
    private Vector2 dir;

    private float initialLightSpotAngle;

    private void Start()
    {
        if (GetComponent<Rigidbody2D>())
        {
            rb2d = GetComponent<Rigidbody2D>();
        }
        else
            Debug.LogError("The character needs a rigidbody.");

        canBeSeen = false;
        // Initialize values for inputs
        moveVertical = 0f;
        moveHorizontal = 0f;
        //inputDelay = 0f; // Can edit this to get a different feel for the character if necessary
        dir = Vector2.zero;
        initialLightSpotAngle = playerLight.spotAngle;
        moveSpeed = startingSpeed;
    }

    // May want to change canBeSeen variable to be tied to velocity instead, as the player could still have canBeSeen be true
    // while holding the run button but not moving, which is not ideal
    private void FixedUpdate()
    {
        GetInput();
        RotatePlayer();
        // Issue with keyboard that you can only register 2 key presses at once, can't hold button while moving diagonally
        if (Input.GetButton("Jump")) // While button is held down, player moves faster
        {
            RunFast();
        }
        else
        {
            Run();
        }
    }

    void GetInput()
    {
        moveVertical = Input.GetAxis("Vertical");
        moveHorizontal = Input.GetAxis("Horizontal");
    }

    void Run()
    {
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        if (moveSpeed > startingSpeed)
        {
            moveSpeed -= acceleration + Time.deltaTime;
            rb2d.velocity = movement * moveSpeed * Time.deltaTime; // Just multiplies in maxSpeed variable to alter speed
        }
        else
        {
            rb2d.velocity = movement * startingSpeed * Time.deltaTime;
            canBeSeen = false;
            //Debug.Log("player is at minimum speed");
        }
        //rb2d.velocity = movement * moveSpeed * Time.deltaTime;
        //Debug.Log("Player is not running.");
        //canBeSeen = false;
        playerLight.spotAngle = initialLightSpotAngle;

    }

    void RunFast()
    {
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        if (moveSpeed < maxSpeed)
        {
            moveSpeed += acceleration + Time.deltaTime;
            rb2d.velocity = movement * moveSpeed * Time.deltaTime; // Just multiplies in maxSpeed variable to alter speed
        }
        else
        {
            rb2d.velocity = movement * maxSpeed * Time.deltaTime;
            //Debug.Log("player is at maximum speed");
        }
        //rb2d.velocity += movement * moveSpeed * maxSpeed * Time.deltaTime;
        //Debug.Log("Player is running.");
        canBeSeen = true;
        playerLight.spotAngle = surfacedLightSpotAngle;
    }

    // Need to fix so it ONLY rotates around the z-axis - Fixed
    void RotatePlayer()
    {
        dir = new Vector2(moveHorizontal, moveVertical);
        if (dir != Vector2.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.FromToRotation(Vector3.right, dir),
                rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, transform.rotation.eulerAngles.z)); // Restricts Slerp rotation to z-axis
        }
    }
}
