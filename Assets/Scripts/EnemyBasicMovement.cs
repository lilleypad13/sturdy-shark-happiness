using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMovement : MonoBehaviour {

    public float moveSpeed = 2.0f;
    public float startingRotationSpeed = 2.0f;
    private float rotationSpeed;
    public float distanceToSeePlayer = 5.0f;
    public Transform waypointGiven;
    public float rotationScaling = 1.0f;

    private Transform myTransform;
    private Transform target;
    //private SpriteRenderer sr;
    //private float distanceToPlayer;
    //public bool canSeePlayer = true;
    public Vector3 dir;

    void Awake()
    {
        myTransform = transform;
    }

    // Sorting Layers
    // Sorting Order values: Enemy = 2; Dark = 3; Player = 4
    void Start()
    {
        rotationSpeed = startingRotationSpeed;
        //sr = GetComponent<SpriteRenderer>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        //canSeePlayer = GetComponent<playerCtrl_EDIT>().canBeSeen;
        //Debug.Log("Waypoint position given is: " + waypointGiven.position);
    }

    void FixedUpdate()
    {
        //DistanceToPlayer();
        //Debug.Log("Distance to player is: " + DistanceToPlayer());
        if (PlayerController.canBeSeen && DistanceToPlayer() < distanceToSeePlayer)
        {
            AggressiveMovement();
        }
        else
        {
            PassiveMovement();
        }
        RotateEnemy();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        WentTooFar(coll);
    }

    void BasicMovement()
    {

        if (PlayerController.canBeSeen)
        {
            dir = target.position - myTransform.position;
            //Debug.Log(gameObject.name + " can see player.");
        }


        //dir.z = 0.0f; // Only needed if objects don't share 'z' value
        if (dir != Vector3.zero)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                Quaternion.FromToRotation(Vector3.right, dir),
                rotationSpeed * Time.deltaTime);
        }

        //Move Towards Target
        //myTransform.position += dir.normalized * moveSpeed * Time.deltaTime;
        //myTransform.position += Vector3.right * moveSpeed * Time.deltaTime;
        myTransform.position += myTransform.right * moveSpeed * Time.deltaTime;

    }

    void AggressiveMovement()
    {
        rotationSpeed += Time.deltaTime * rotationScaling;
        dir = target.position - myTransform.position;
        //Debug.Log(gameObject.name + " can see player.");
        myTransform.position += myTransform.right * moveSpeed * Time.deltaTime;
    }

    void PassiveMovement()
    {
        rotationSpeed = startingRotationSpeed;
        // Need to set dir to aim at waypoint
        //dir = target.position - myTransform.position; // Use this but adapt to waypoint instead of target
        if (waypointGiven != null)
        {
            dir = waypointGiven.position - myTransform.position;
        }
        myTransform.position += myTransform.right * moveSpeed * Time.deltaTime;
    }

    void RotateEnemy()
    {
        if (dir != Vector3.zero)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                Quaternion.FromToRotation(Vector3.right, dir),
                rotationSpeed * Time.deltaTime);
        }
    }

    void WentTooFar(Collider2D grave)
    {
        if (grave.gameObject.CompareTag("Grave"))
        {
            Destroy(gameObject);
        }
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, target.position);
    }
}
