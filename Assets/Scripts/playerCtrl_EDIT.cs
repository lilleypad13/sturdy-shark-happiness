//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class playerCtrl_EDIT : MonoBehaviour {

//    // Use this for initialization
//    public float speed = 6f;
//    public static bool haveWater;
//    public static bool canBeSeen;

//    public AudioClip gotWater; // added Audio
    
//    private GameObject[] water;
//    private float XdistW, YdistW;
//    private float[] XdistF, YdistF;
//    private GameObject[] fire,fireList;

//    private AudioSource source; // added Audio

//    void Start () {
//        haveWater = false;
//        canBeSeen = false;
//        source = GetComponent<AudioSource>(); // For Audio
//    }
	
//	// Update is called once per frame
//	void Update () {
//        float X = Input.GetAxis("Horizontal");
//        float Y = Input.GetAxis("Vertical");

//        transform.Translate(new Vector3(X, Y) * Time.deltaTime * speed);

//        if (this.gameObject.transform.position.y > 4.3f)
//        {
//            transform.position = new Vector3(transform.position.x, 4.3f, transform.position.z);
//        }
//        if (this.gameObject.transform.position.y < -4.3f)
//        {
//            transform.position = new Vector3(transform.position.x, -4.3f, transform.position.z);
//        }
//        if (this.gameObject.transform.position.x > 8.25f)
//        {
//            transform.position = new Vector3(8.25f, transform.position.y, transform.position.z);
//        }
//        if (this.gameObject.transform.position.x < -8.25f)
//        {
//            transform.position = new Vector3(-8.25f, transform.position.y, transform.position.z);
//        }
//        pickup();
        
//        //Debug.Log(haveWater);
        

//    }

//    //private void OnCollisionEnter2D(Collision2D collision)
//    //{
//    //    if (collision.gameObject.CompareTag("Campfire"))
//    //    {
//    //        if(haveWater == true)
//    //        {
//    //            Destroy(collision.gameObject);
//    //            haveWater = false;
//    //        }
//    //    }
        
//    //}

//    private void OnTriggerEnter2D(Collider2D coll)
//    {
//        FireCollision(coll);
//        IsVisible(coll);
//    }

//    private void OnTriggerExit2D(Collider2D coll)
//    {
//        IsInvisible(coll);
//    }

//    void FireCollision(Collider2D fire)
//    {
//        if (fire.gameObject.CompareTag("Campfire")){
//            if(haveWater == true)
//            {
//                fire.GetComponent<FireBehaviorScript>().fireHealth--;
//                Debug.Log("This fire's health is: " + fire.GetComponent<FireBehaviorScript>().fireHealth);
//                haveWater = false;
//            }
//        }
//    }

//    void IsVisible(Collider2D light)
//    {
//        if (light.gameObject.CompareTag("Light"))
//        {
//            canBeSeen = true;
//            Debug.Log("Player can be seen.");
//        }
//    }

//    void IsInvisible(Collider2D light)
//    {
//        if (light.gameObject.CompareTag("Light"))
//        {
//            canBeSeen = false;
//            Debug.Log("Player can no longer be seen.");
//        }
//    }

//    void pickup()
//    {
//        water = GameObject.FindGameObjectsWithTag("WaterSpawn");

//        XdistW = Mathf.Abs(water[spawnWater.posNum].transform.position.x - this.gameObject.transform.position.x);
//        YdistW = Mathf.Abs(water[spawnWater.posNum].transform.position.y - this.gameObject.transform.position.y);
//        if (XdistW < 0.65 && YdistW <0.85 )
//        {
//            if(Input.GetKey(KeyCode.Space))
//            {
//                haveWater = true;
//                source.PlayOneShot(gotWater, 1f); // added audio cue
//                spawnWater.waterNum = 0;
//            }
//        }
//    }

   
//}
