using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMovement : MonoBehaviour {

    //public float moveSpeed = 2.0f;
    //public float startingRotationSpeed = 2.0f;
    //private float rotationSpeed;
    //public float distanceToSeePlayer = 5.0f;
    //public Transform waypointGiven;
    //public float rotationScaling = 1.0f;

    //private Transform myTransform;
    //private Transform target;
    public Vector3 direction;

    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMoveThreshold = 0.5f;

    public Transform target;
    public float speed = 20.0f;
    public float turnSpeed = 3.0f;
    public float turnDistance = 5.0f;
    public float stoppingDistance = 10.0f;

    Path path;

    IEnumerator followRoutine;
    IEnumerator updatePathRoutine;

    //void Awake()
    //{
    //    myTransform = transform;
    //}

    // Sorting Layers
    // Sorting Order values: Enemy = 2; Dark = 3; Player = 4
    void Start()
    {
        //rotationSpeed = startingRotationSpeed;
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        //target = player.transform;

        followRoutine = FollowPath();
        updatePathRoutine = UpdatePath();
        StartCoroutine(updatePathRoutine);
    }

    /*
     * Gives a path to the unit only if a path was successfully found
     */
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            Debug.Log("OnPathFound run");
            path = new Path(waypoints, transform.position, turnDistance, stoppingDistance);
            Debug.Log("Number of waypoints in new path is:" + waypoints.Length);
            //StopCoroutine("FollowPath");
            //StartCoroutine("FollowPath");
            StopCoroutine(followRoutine);
            followRoutine = FollowPath();
            StartCoroutine(followRoutine);
        }
    }

    /*
     * Lets the unit request a new path if the target has moved considerably and some time has passed
     */
    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(0.3f);
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        float squareMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPositionOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((target.position - targetPositionOld).sqrMagnitude > squareMoveThreshold)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPositionOld = target.position;
            }
        }
    }

    /*
     * Responsible for actually moving unit along its path
     */
    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        //transform.LookAt(path.lookPoints[0]);
        Vector3 direction = (path.lookPoints[0] - transform.position).normalized;
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        float speedPercent = 1.0f; // Used to control unit speed throughout path
        Debug.Log("Starting Follow Path procedure.");
        while (followingPath)
        {
            Vector2 position2D = new Vector2(transform.position.x, transform.position.y);
            //Debug.Log("Path index is: " + pathIndex);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(position2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                if (pathIndex >= path.slowDownIndex && stoppingDistance > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(position2D) / stoppingDistance);
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }

                // Slows unit down as it reaches the last waypoint
                speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(position2D) / stoppingDistance);

                //Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                direction = (path.lookPoints[pathIndex] - transform.position).normalized;
                rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, rot_z);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                //transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
                transform.Translate(Vector3.right * Time.deltaTime * speed * speedPercent, Space.Self);
            }
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }

    //void FixedUpdate()
    //{
    //    if (PlayerController.canBeSeen && DistanceToPlayer() < distanceToSeePlayer)
    //    {
    //        AggressiveMovement();
    //    }
    //    else
    //    {
    //        PassiveMovement();
    //    }
    //    RotateEnemy();
    //}

    //private void OnTriggerEnter2D(Collider2D coll)
    //{
    //    WentTooFar(coll);
    //}

    //void BasicMovement()
    //{

    //    if (PlayerController.canBeSeen)
    //    {
    //        direction = target.position - myTransform.position;
    //        //Debug.Log(gameObject.name + " can see player.");
    //    }


    //    //dir.z = 0.0f; // Only needed if objects don't share 'z' value
    //    if (direction != Vector3.zero)
    //    {
    //        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
    //            Quaternion.FromToRotation(Vector3.right, direction),
    //            rotationSpeed * Time.deltaTime);
    //    }

    //    //Move Towards Target
    //    myTransform.position += myTransform.right * moveSpeed * Time.deltaTime;

    //}

    //// Aggressive State
    //void AggressiveMovement()
    //{
    //    rotationSpeed += Time.deltaTime * rotationScaling;
    //    direction = target.position - myTransform.position;
    //    //Debug.Log(gameObject.name + " can see player.");
    //    myTransform.position += myTransform.right * moveSpeed * Time.deltaTime;
    //}

    //// Passive State
    //void PassiveMovement()
    //{
    //    rotationSpeed = startingRotationSpeed;
    //    // Need to set dir to aim at waypoint
    //    //dir = target.position - myTransform.position; // Use this but adapt to waypoint instead of target
    //    if (waypointGiven != null)
    //    {
    //        direction = waypointGiven.position - myTransform.position;
    //    }
    //    myTransform.position += myTransform.right * moveSpeed * Time.deltaTime;
    //}

    //void RotateEnemy()
    //{
    //    if (direction != Vector3.zero)
    //    {
    //        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
    //            Quaternion.FromToRotation(Vector3.right, direction),
    //            rotationSpeed * Time.deltaTime);
    //    }
    //}

    //void WentTooFar(Collider2D grave)
    //{
    //    if (grave.gameObject.CompareTag("Grave"))
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, target.position);
    }
}
