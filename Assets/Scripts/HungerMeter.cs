using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerMeter : MonoBehaviour {

	
	void Update ()
    {
        //currentHealth = FindObjectOfType<PlayerEvents>().currentHealth;
        transform.localScale = new Vector3(PlayerHitboxScript.playerHunger / PlayerHitboxScript.startingHunger,transform.localScale.y, 1.0f);
        //if (currentHealth > maxHealth) // Keeps progress in check (basically stops "increasing" after player has met goal
        //{
        //    currentHealth = maxHealth;
        //}
    }
}
