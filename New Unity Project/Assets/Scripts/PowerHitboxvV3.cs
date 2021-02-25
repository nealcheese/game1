using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHitboxvV3 : MonoBehaviour
{

    private Rigidbody2D rb;
    private Collider2D kc;
    public GameObject LineHitBox;
    public GameObject LineFollow;
    private float startTime;
    Vector2[] line_collider_points;
    Vector2[] line_follow_points;
    private int I;
    public float speed = 40f;
    float journeyLength;
    Vector3 start_position;
    private bool online = false;
    private float fractionOfJourney = 0.0f;
    float hit_x;
    float hit_y;
    bool cunt = false;
    bool x_here = false;
    bool y_here = false;

    void Start()
    {
        //Stops OnTrigger functions from starting due to the raycasts hitting the character collider from inside
        Physics2D.queriesStartInColliders = false;
        rb = GetComponent<Rigidbody2D>();
        kc = GetComponent<Collider2D>();
    }
  
    void Update()
    {
         //Debug.Log(cunt);
        
        if(online == false)
        {
            //Debug.Log(online);
            
            //Set the starting points for the raycasts in each direction (from the centre of the character, slightly offset)
            Vector3 raycast_right_start = new Vector3 (rb.transform.position.x, rb.transform.position.y, rb.transform.position.z);
            Vector3 raycast_left_start = new Vector3 (rb.transform.position.x, rb.transform.position.y, rb.transform.position.z);
            Vector3 raycast_down_start = new Vector3 (rb.transform.position.x, rb.transform.position.y+1f, rb.transform.position.z);

            //generate the raycasts and store the hits
            RaycastHit2D hit_down = Physics2D.Raycast(raycast_down_start, Vector2.down, Mathf.Infinity, 1<<LayerMask.NameToLayer("LineHitBox"));
            RaycastHit2D hit_right = Physics2D.Raycast(raycast_right_start, Vector2.right, Mathf.Infinity,1<<LayerMask.NameToLayer("LineHitBox"));
            RaycastHit2D hit_left = Physics2D.Raycast(raycast_left_start, Vector2.left, Mathf.Infinity,1<<LayerMask.NameToLayer("LineHitBox"));
            
            Debug.DrawRay(raycast_down_start, Vector3.down, Color.white, 0.01f, true);
            Debug.DrawRay(raycast_right_start, Vector3.right, Color.white, 0.01f, true);
            Debug.DrawRay(raycast_left_start, Vector3.left, Color.white, 0.01f, true);
            //Debug.Log(hit_down.distance);
            //Debug.Log(hit_right.distance);
            //Debug.Log(hit_left.distance);

            //get the line collider points and store them in a 2D Vector array
            EdgeCollider2D line_collider = LineHitBox.GetComponent<EdgeCollider2D>(); 
            EdgeCollider2D line_follow = LineFollow.GetComponent<EdgeCollider2D>();
            line_follow_points = line_follow.points;
            line_collider_points = line_collider.points;
     
            //if the right raycast is closest to the line, store the hit point and start the function that finds where the hit is on the line
            if(hit_right.collider != null && hit_right.distance > 0.0001f && hit_right.distance < 0.5f)
            {


                Debug.DrawRay(raycast_right_start, Vector3.right, Color.red, 1f, true);

                
                //Debug.Log(hit_right.collider.gameObject.tag);

                    Debug.Log("Raycast_Right");
                    hit_x = hit_right.point.x;
                    hit_y = hit_right.point.y;
                    
                    online = true;
                    cunt = true;
                    //Debug.Log(cunt);
                    rb.drag = 1;
                    Point_Find();

            }

            //if the left raycast is closest to the line, store the hit point and start the function that finds where the hit is on the line
            if(hit_left.collider != null && hit_left.distance > 0.0001f && hit_left.distance < 0.5f)
            {


                Debug.DrawRay(raycast_left_start, Vector3.left, Color.red, 1f, true);


                //Debug.Log(hit_left.collider.gameObject.tag);

                    Debug.Log("Raycast_Left");
                    hit_x = hit_left.point.x;
                    hit_y = hit_left.point.y;
                    
                    online = true;
                    cunt = true;
                    //Debug.Log(cunt);
                    rb.drag = 1;
                    Point_Find();

            }
            //if the down raycast is closest to the line, store the hit point and start the function that finds where the hit is on the line
            if(hit_down.collider != null && hit_down.distance > 0.0001f && hit_down.distance < 2f)
            {


                    Debug.Log("Raycast_Down");
                    Debug.DrawRay(raycast_down_start, Vector3.down, Color.red, 1f, true);
                
                    Debug.Log(hit_down.collider.gameObject.tag);

                        hit_x = hit_down.point.x;
                        hit_y = hit_down.point.y;
                        
                        online = true;
                        cunt = true;
                        //Debug.Log(cunt);
                        rb.drag = 1;
                        Point_Find();

            }
        }













        //if true, allow the line following function to start. Online = true when the nearest line collider point is detected
        if (cunt == true && online == true && line_follow_points.Length >= 1)
        {
            x_here = false;
            y_here = false;
           
            //Debug.Log(I);
            //Follow each point of the line from the starting line collider point to the end of the line
            //when reached the end of the line, no longer online
           
            if(fractionOfJourney < 0.99f)
            {
                //check if lin collider point is near wall

                if(I < line_follow_points.Length-2 && line_follow_points[I] != null)
                {
                    //Debug.Log(I);
                    //Debug.Log(line_follow_points[I]);
                    journeyLength = Vector2.Distance(start_position, line_follow_points[I]);
                    float distCovered = (Time.time - startTime) * speed;
                    
                    fractionOfJourney = distCovered / journeyLength;
                    
                    rb.transform.position = Vector2.Lerp(start_position, line_follow_points[I], fractionOfJourney);
                    cunt = true;
                }
            }
           
            //Lerp function starts again for the next point
            if(fractionOfJourney >= 0.90f)
            {
                Debug.Log("!!");
                //Debug.Log(fractionOfJourney);
                start_position = line_follow_points[I];
                fractionOfJourney = 0;
                I++;
                startTime = Time.time;
            }
           
            //Ends when reached the end of the list array
            if(I >= line_follow_points.Length-2)
            {
                Debug.Log("???");
                
                cunt = false;
                StartCoroutine(LineDeactivate());
                Vector2 velocity2 = line_follow_points[line_follow_points.Length-2] - line_follow_points[line_follow_points.Length-3];
                if(Mathf.Abs(velocity2.x) <= Mathf.Abs(velocity2.y))
                {
                    rb.velocity = new Vector2 ((velocity2.x/Mathf.Abs(velocity2.y))*20, (velocity2.y/Mathf.Abs(velocity2.y))*20);
                }
                if(Mathf.Abs(velocity2.x) > Mathf.Abs(velocity2.y))
                {
                    rb.velocity = new Vector2 ((velocity2.x/Mathf.Abs(velocity2.x))*20, (velocity2.y/Mathf.Abs(velocity2.x))*20);
                }
                Debug.Log(rb.velocity);
                fractionOfJourney = 0;
                rb.gravityScale = 10f;
                
            }
            
        }
    }

    //when the character collides with something
    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "LineHitBox" && online == true)
        {
           // online = false;
           StartCoroutine(LineDeactivate());
            cunt = false;
            rb.gravityScale = 10f;
            Debug.Log("Trigger");
            Vector2 velocity2 = line_collider_points[line_collider_points.Length-2] - line_collider_points[line_collider_points.Length-3];
            if(Mathf.Abs(velocity2.x) <= Mathf.Abs(velocity2.y))
                {
                    rb.velocity = new Vector2 ((velocity2.x/Mathf.Abs(velocity2.y))*20, (velocity2.y/Mathf.Abs(velocity2.y))*20);
                }
                if(Mathf.Abs(velocity2.x) > Mathf.Abs(velocity2.y))
                {
                    rb.velocity = new Vector2 ((velocity2.x/Mathf.Abs(velocity2.x))*20, (velocity2.y/Mathf.Abs(velocity2.x))*20);
                }
            //Debug.Log(collider.gameObject.tag);
            //checking the collision is with the line
        }
        
    }

    void Point_Find()
    {
        //Debug.Log("Point_Find");
        //search through the array of line collider points to see which is closest/equal to our newly generated hit point (from closest raycast)
        for(int i = 1; i < line_collider_points.Length; i++)
        {
            //Debug.Log("Point_Find");
            //Y CHECK FOR ASCENDING LINE: Determine whether the hit point y value is in between the y values of two consecutive line collider points
            if (line_collider_points[i-1].y <= hit_y && hit_y <= line_collider_points[i].y)
            {
                y_here = true;
            }

            //X CHECK FOR ASCENDING LINE
            if (line_collider_points[i-1].x <= hit_x && hit_x <= line_collider_points[i].x)
            {
                x_here = true;
            }
            
            //Y CHECK FOR DESCENDING LINE
            if (line_collider_points[i-1].y >= hit_y && hit_y >= line_collider_points[i].y)
            {
                 y_here = true;
            }

            //X CHECK FOR DESCENDING LINE
            if (line_collider_points[i-1].x >= hit_x && hit_x >= line_collider_points[i].x)
            {
                x_here = true;
            }

            if(y_here == true && x_here == true)
            {
                //store the point of the line collider and start the line folowing function
                I = i;
                start_position = rb.transform.position;
                //Debug.Log("pointfound");
                startTime = Time.time;
                online = true;
                return;
            }
        }
    }

    IEnumerator LineDeactivate()
    {
        yield return 0;
        yield return 0;
        yield return 0;
        yield return 0;
        yield return 0;
        yield return 0;
        yield return 0;
        yield return 0;
        yield return 0;
        yield return 0;
        online = false;
    }

}