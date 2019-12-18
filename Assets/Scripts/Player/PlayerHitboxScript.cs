using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitboxScript : MonoBehaviour {

    public GameObject gameManager;

    public static int playerScore;
    public static float playerHunger;

    public static float startingHunger;

    public float hungerFactor = 1.0f;
    public float hungerReplenishedPerEat = 5.0f;
    public AudioSource source;
    public AudioClip biteSound;

    private void Start()
    {
        playerScore = 0;
        playerHunger = 100;
        startingHunger = playerHunger;
    }

    private void Update()
    {
        if (playerHunger > 0)
        {
            if (playerHunger > startingHunger)
            {
                playerHunger = startingHunger; // Keeps player from being "overfull"
            }
            playerHunger -= Time.deltaTime * hungerFactor;
        }
        else
        {
            Debug.Log("You have died of hunger.");
            gameManager.GetComponent<GameManager>().LoseGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (GameManager.gameHasEnded == false)
        {
            HitEnemy(coll);
            HitEnemyWeakPoint(coll);
        }
    }

    void HitEnemy(Collider2D enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            Debug.Log("You have died by enemy.");
            gameManager.GetComponent<GameManager>().LoseGame();
        }
    }

    void HitEnemyWeakPoint(Collider2D weakPoint)
    {
        if (weakPoint.CompareTag("Weak"))
        {
            Destroy(weakPoint.transform.parent.gameObject);
            playerHunger += hungerReplenishedPerEat;
            playerScore++;
            source.PlayOneShot(biteSound, 1f);
            //Debug.Log("Player score: " + playerScore);
            //Debug.Log("Killed enemy.");
        }
    }
}
