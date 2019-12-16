using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviorScript : MonoBehaviour {

    

    public GameObject darkOutterRing;
    public GameObject darkInnerRing;

    public int fireHealth = 2;

    // When health = 0; use > Destroy(transform.parent.gameObject);

    private void Update()
    {
        ActivateOutterRing();
        CheckIfDestroyed();
    }

    void CheckIfDestroyed()
    {
        if(fireHealth <= 0)
        {
            ActivateInnerRing();
            Destroy(transform.parent.gameObject);
        }
    }

    void ActivateOutterRing()
    {
        if (fireHealth <= 1)
        {
            darkOutterRing.gameObject.SetActive(true);
        }
    }

    void ActivateInnerRing()
    {
        darkInnerRing.gameObject.SetActive(true);
    }
}
