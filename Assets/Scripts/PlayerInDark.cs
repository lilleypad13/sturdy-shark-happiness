using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInDark : MonoBehaviour {

    private SpriteRenderer sr;
    private Color originalColor;
    private Color currentColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start () {
        originalColor = sr.color;
        currentColor = originalColor;
        currentColor.a = 0.2f;
        sr.color = currentColor;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DamagedByEnemy(other);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {

        PlayerEnteredLight(coll);
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        //if (coll.gameObject.CompareTag("Dark"))
        //{
        //    currentColor.a = 1f;
        //    sr.color = currentColor;
        //}
        if (coll.gameObject.CompareTag("Light"))
        {
            currentColor.a = 0.2f;
            sr.color = currentColor;
        }
    }

    private void PlayerEnteredLight(Collider2D light)
    {
        if (light.gameObject.CompareTag("Light"))
        {
            currentColor.a = 1f;
            sr.color = currentColor;
        }
    }

    private void DamagedByEnemy(Collision2D enemy)
    {
        if (enemy.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
