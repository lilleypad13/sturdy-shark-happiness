using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyScript : MonoBehaviour {

    public GameObject enemyObject;
    public Transform[] spawns;

    private int enemyCounter = 0;

    //[Range(0.0f, 1.0f)]
    //public float spawnRate = 0.5f; // Probability an enemy will spawn
    //public float spawnIncrease = 0f;
    //public int countToIncreaseSpawnRate = 0; // Sets number of times for function to repeat before increasing the spawn rate
    //public float timeToStartSpawning = 1.0f;
    public float timeBetweenSpawnAttempts = 1.0f;

    public float minTimeBetweenSpawns = 1.0f;

    private float counter = 0;
    public float timeToIncreaseSpawnRate;
    public float amountToIncreaseSpawnRate;

    //private float spawnRoll;
    //private int count;


	void Start () {
        //InvokeRepeating("SpawnEnemy", timeToStartSpawning, timeBetweenSpawnAttempts);
        //InvokeRepeating("SpawnControl", timeToStartSpawning, timeBetweenSpawnAttempts);
        InvokeRepeating("SpawnControl", timeBetweenSpawnAttempts, timeBetweenSpawnAttempts);
    }

    private void Update()
    {
        if (!IsInvoking("SpawnControl"))
        {
            InvokeRepeating("SpawnControl", timeBetweenSpawnAttempts, timeBetweenSpawnAttempts);
        }
        counter += Time.deltaTime;
        if (timeBetweenSpawnAttempts > minTimeBetweenSpawns)
        {
            if (counter >= timeToIncreaseSpawnRate)
            {
                timeBetweenSpawnAttempts -= amountToIncreaseSpawnRate;
                counter = 0;
                CancelInvoke();
            }
        }
    }

    //void SpawnEnemy()
    //{
    //    count++;
    //    if (spawnRate < 1)
    //    {
    //        if (count >= countToIncreaseSpawnRate)
    //        {
    //            spawnRate += spawnIncrease;
    //            count = 0;
    //        }
    //    }
    //    spawnRoll = Random.Range(0f, 1f);
    //    if (spawnRoll <= spawnRate)
    //    {
    //        Instantiate(enemyObject, transform.position, Quaternion.identity);
    //    }
    //}

    public void SpawnControl()
    {
        //bool objectPlaced = false;
        List<Transform> freeSpawnPoints = new List<Transform>(spawns);  // Creates new array of the transform values of the list of objects dragged onto this script
        //List<GameObject> freeSpawnPoints = new List<GameObject>(spawnPoints);  // Creates new array of gameObjects from the list of objects dragged onto this script

        // Following for loop goes through the newly created array and resets all the isOccupied values of the spawn points to "false"
        //for (i = 0; i < freeSpawnPoints.Count; i++)
        //{
        //    freeSpawnPoints[i].GetComponent<SpawnPointPickupRock>().isOccupied = false;
        //}
        int index = Random.Range(0, freeSpawnPoints.Count);  // Picks a random index number between 0 and the size of the previous array (randomly selects transform from array)
        Transform selectedSpawn = freeSpawnPoints[index];
        //Transform pos = selectedSpawn.GetComponent<Transform>();  // Finds the transform value of the selected game object in the array, and sets that value equal to pos
        GameObject newEnemy = Instantiate(enemyObject, selectedSpawn.position, selectedSpawn.rotation);  // Creates an object at the selected position
        enemyCounter++;
        newEnemy.name = "Enemy" + enemyCounter + "From" + selectedSpawn.gameObject.name;
        EnemyBasicMovement enemy = newEnemy.GetComponent<EnemyBasicMovement>();
        enemy.target = GameManager.Instance.player.transform;
        enemy.enabled = true;
        //newEnemy.GetComponent<EnemyBasicMovement>().waypointGiven = selectedSpawn.gameObject.GetComponent<SpawnPointScript>().waypointLocation;
        //gearBaby.GetComponent<GearTwoPartMesh>().numberOfTeeth = numberOfTeeth;
        //Debug.Log("Object placed at: " + selectedSpawn.name);
        //bool occupiedCheck = selectedSpawn.GetComponent<SpawnPointPickupRock>().isOccupied;  // Sets occupiedCheck to whatever value isOccupied is on the selected game object
        //if (occupiedCheck == false)
        //{
        //    Transform pos = selectedSpawn.GetComponent<Transform>();  // Finds the transform value of the selected game object in the array, and sets that value equal to pos
        //    Instantiate(pickupRockObject, pos.position, pos.rotation);  // Creates an object at the selected position
        //    objectPlaced = true;
        //    Debug.Log("Object placed at: " + selectedSpawn.name);
        //}
        //else
        //{
        //    freeSpawnPoints.RemoveAt(index); // This means occupiedCheck returned true, so the spawn point is already occupied. This will remove that occupied point from the array for the next random selection.
        //}
        //if (freeSpawnPoints.Count <= 0) // This takes care of the case where the loop has run through all possible spawn points and they are all 
        //                                // occupied, eliminating all elements from the array. This will end the while loop and the function so that it may progress in time.
        //{
        //    objectPlaced = true;
        //}
        //Debug.Log(index);
        //Debug.Log(selectedSpawn.GetComponent<SpawnPointPickupRock>().isOccupied);
    }
}
