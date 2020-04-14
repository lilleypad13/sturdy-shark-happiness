using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region Variables
    // Public variables
    public float startingSpeed = 1.0f;
    public float maxSpeed = 2.0f;
    public float rotationSpeed = 2.0f;
    public float acceleration = 2.0f;
    public float rotationTime = 1.0f;
    public float interpolant = 0.5f;

    public Light playerLight;
    public float surfacedLightSpotAngle = 30f;

    // Public static variables
    public static bool canBeSeen;

    // Private variables
    private float moveSpeed;
    private float moveVertical;
    private float moveHorizontal;
    private float inputDelay;
    private Rigidbody rb;
    private Vector3 direction;

    private float initialLightSpotAngle;
    #endregion


    private void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            rb = GetComponent<Rigidbody>();
        }
        else
            Debug.LogError("The character needs a rigidbody.");

        canBeSeen = false;
        // Initialize values for inputs
        moveVertical = 0f;
        moveHorizontal = 0f;
        //inputDelay = 0f; // Can edit this to get a different feel for the character if necessary
        direction = Vector3.zero;
        initialLightSpotAngle = playerLight.spotAngle;
        moveSpeed = startingSpeed;
    }

    // May want to change canBeSeen variable to be tied to velocity instead, as the player could still have canBeSeen be true
    // while holding the run button but not moving, which is not ideal
    private void FixedUpdate()
    {
        GetInput();
        RotatePlayer();
        if (Input.GetButton("Jump")) // While button is held down, player moves faster
        {
            RunFast();
        }
        else
        {
            Run();
        }
    }

    /*
     * Receives input from player and generates a normalized directional vector from it
     */
    void GetInput()
    {
        moveVertical = Input.GetAxis("Vertical");
        moveHorizontal = Input.GetAxis("Horizontal");

        direction = new Vector3(moveHorizontal, moveVertical, 0.0f);
        direction = Vector3.Normalize(direction);
    }

    /*
     * Default movement option
     */
    void Run()
    {
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        if (moveSpeed > startingSpeed)
        {
            moveSpeed -= acceleration + Time.deltaTime;
            rb.velocity = movement * moveSpeed * Time.deltaTime; // Just multiplies in maxSpeed variable to alter speed
        }
        else
        {
            rb.velocity = movement * startingSpeed * Time.deltaTime;
            canBeSeen = false;
            //Debug.Log("player is at minimum speed");
        }
        
        playerLight.spotAngle = initialLightSpotAngle;

    }

    /*
     * Fast movement option
     */
    void RunFast()
    {
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        if (moveSpeed < maxSpeed)
        {
            moveSpeed += acceleration + Time.deltaTime;
            rb.velocity = movement * moveSpeed * Time.deltaTime; // Just multiplies in maxSpeed variable to alter speed
        }
        else
        {
            rb.velocity = movement * maxSpeed * Time.deltaTime;
            //Debug.Log("player is at maximum speed");
        }
        //Debug.Log("Player is running.");
        canBeSeen = true;
        playerLight.spotAngle = surfacedLightSpotAngle;
    }

    // Need to fix so it ONLY rotates around the z-axis - Fixed
    void RotatePlayer()
    {
        //direction = new Vector3(moveHorizontal, moveVertical, 0.0f);
        if (direction != Vector3.zero)
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

            //transform.up = Vector3.Lerp(transform.up, direction, rotationTime * Time.deltaTime);
            transform.up = Vector3.Lerp(transform.up, direction, interpolant);

            //transform.rotation = Quaternion.Slerp(transform.rotation,
            //    Quaternion.FromToRotation(Vector3.up, direction),
            //    rotationSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, transform.rotation.eulerAngles.z)); // Restricts Slerp rotation to z-axis
        }


    }
}
