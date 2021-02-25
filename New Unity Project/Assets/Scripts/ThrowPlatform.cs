using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPlatform : MonoBehaviour
{ 

    public GameObject platform;
    public GameObject platformcollider;
    public Transform character;
    public GameObject kate;
    Camera thisCamera;
    
    public Rigidbody2D rb;
    
    private bool thrown = false;
    
    private float startTime;
    float speed;
    Vector3 startposition;
    Vector3 mouseWorld;
    float journeyLength;

   void Awake()
   {
       thisCamera = Camera.main;
   }
    
    void Start()
    {
        float x = platform.transform.position.x;
        float y = platform.transform.position.y;
        float z = platform.transform.position.z;
        platform.transform.position = new Vector3(x, y, z);
        rb.gravityScale = 0.0f;

    }

    void Update()
    {

        float speed = 40f;
 

        if(Input.GetMouseButtonDown(1))
        {
            
            
            Vector3 mousePos = Input.mousePosition;
            mouseWorld = thisCamera.ScreenToWorldPoint(mousePos);
            mouseWorld.z = platform.transform.position.z;

            startposition = character.transform.position;
            
            journeyLength = Vector3.Distance(startposition, mouseWorld);
            if(journeyLength > 10f)
            {
                Vector3 distance_vector =  startposition - mouseWorld;
                float x = distance_vector.x*(10/journeyLength);
                float y = distance_vector.y*(10/journeyLength);

                distance_vector = new Vector3(x, y, mouseWorld.z);
                mouseWorld = startposition - distance_vector;
                Debug.Log(mouseWorld);
                journeyLength = Vector3.Distance(startposition, mouseWorld);
            }
            thrown = true;
            startTime = Time.time;

            StartCoroutine(collisionignore());

        }  


        if(thrown == true)
        {
            float distCovered = (Time.time - startTime) * speed;
        
            //Debug.Log(distCovered);
            float fractionOfJourney = distCovered / journeyLength;
            platform.transform.position = Vector3.Lerp(startposition, mouseWorld, fractionOfJourney);
        }






    }

    IEnumerator collisionignore()
    {
        Debug.Log("collisionignore");
        Physics2D.IgnoreCollision(kate.GetComponent<Collider2D>(), platformcollider.GetComponent<Collider2D>());
        yield return new WaitForSeconds(0.2f);
        Physics2D.IgnoreCollision(kate.GetComponent<Collider2D>(), platformcollider.GetComponent<Collider2D>(), false);
        
    }
}
