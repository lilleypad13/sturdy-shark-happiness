using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMovement : MonoBehaviour {

    public float moveSpeed = 2.0f;
    public int rotationSpeed = 2;

    public int initialSortingLayer;
    public int currentSortingLayer;

    private Transform myTransform;
    private Transform target;
    private SpriteRenderer sr;
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
        sr = GetComponent<SpriteRenderer>();
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        target = go.transform;
        //canSeePlayer = GetComponent<playerCtrl_EDIT>().canBeSeen;
        dir = Vector3.up;
        initialSortingLayer = sr.sortingOrder;
        currentSortingLayer = initialSortingLayer;
        Debug.Log("Initial sorting order value is: " + initialSortingLayer);
    }

    void FixedUpdate()
    {
        if (PlayerController.canBeSeen)
        {
            AggressiveMovement();
        }
        else
        {
            PassiveMovement();
        }
        RotateEnemy();
    }

    void BasicMovement()
    {

        if (PlayerController.canBeSeen)
        {
            dir = target.position - myTransform.position;
            Debug.Log(gameObject.name + " can see player.");
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
        dir = target.position - myTransform.position;
        Debug.Log(gameObject.name + " can see player.");
        myTransform.position += myTransform.right * moveSpeed * Time.deltaTime;
    }

    void PassiveMovement()
    {
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
}
