using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHitboxV2 : MonoBehaviour
{

    private Rigidbody2D rb;
    public GameObject LineHitBox;
    private float startTime;
    Vector2[] line_collider_points;
    private int I;
    public float speed = 40f;
    float journeyLength;
    Vector3 start_position;
    private bool online = false;
    private float fractionOfJourney = 0.0f;
    float hit_x;
    float hit_y;

    void Start()
    {
        //Stops OnTrigger functions from starting due to the raycasts hitting the character collider from inside
        Physics2D.queriesStartInColliders = false;
        rb = GetComponent<Rigidbody2D>();
    }
  
    void Update()
    {
        
        //if true, allow the line following function to start. Online = true when the nearest line collider point is detected
        if (online == true)// && I != 0)
        {
           

            //Follow each point of the line from the starting line collider point to the end of the line
            //when reached the end of the line, no longer online
           
            if(fractionOfJourney < 0.99f)
            {
                
                
                journeyLength = Vector3.Distance(start_position, line_collider_points[I]);
                float distCovered = (Time.time - startTime) * speed;
                Debug.Log(line_collider_points[I]);
                fractionOfJourney = distCovered / journeyLength;
                rb.transform.position = Vector3.Lerp(start_position, line_collider_points[I], fractionOfJourney);
                
            }
           
            //Lerp function starts again for the next point
            if(fractionOfJourney >= 0.99f)
            {
                start_position = line_collider_points[I];
                I++;
                startTime = Time.time;
            }
           
            //Ends when reached the end of the list array
            if(I >= line_collider_points.Length-1)
            {
                online = false;
                fractionOfJourney = 0;
            }
            
        }
    }

    //when the character collides with something
    void OnTriggerStay2D(Collider2D collider)
    {
        //Debug.Log("Trigger");
        //Debug.Log(collider.gameObject.tag);
        //checking the collision is with the line
        if (collider.gameObject.tag == "LineHitBox" && online == false)
        {
            //Set the starting points for the raycasts in each direction (from the centre of the character, slightly offset)
            Vector3 raycast_right_start = new Vector3 (rb.transform.position.x, rb.transform.position.y, rb.transform.position.z);
            Vector3 raycast_left_start = new Vector3 (rb.transform.position.x, rb.transform.position.y, rb.transform.position.z);
            Vector3 raycast_down_start = new Vector3 (rb.transform.position.x, rb.transform.position.y, rb.transform.position.z);

            //generate the raycasts and store the hits
            RaycastHit2D hit_down = Physics2D.Raycast(raycast_down_start, new Vector2(0f,-2f));
            RaycastHit2D hit_right = Physics2D.Raycast(raycast_right_start, new Vector2(2f,0f));
            RaycastHit2D hit_left = Physics2D.Raycast(raycast_left_start, new Vector2(-2f,0f));

            Debug.DrawRay(raycast_down_start, Vector3.down, Color.white, 1f, true);
            Debug.DrawRay(raycast_right_start, Vector3.right, Color.white, 1f, true);
            Debug.DrawRay(raycast_left_start, Vector3.left, Color.white, 1f, true);
            //Debug.Log(hit_down.distance);
            //Debug.Log(hit_right.distance);
            //Debug.Log(hit_left.distance);

            //get the line collider points and store them in a 2D Vector array
            EdgeCollider2D line_collider = LineHitBox.GetComponent<EdgeCollider2D>();
            line_collider_points = line_collider.points;
     
            //if the right raycast is closest to the line, store the hit point and start the function that finds where the hit is on the line
            if(hit_right.distance <= 3f && hit_left.distance > 3f && hit_right.collider.gameObject.tag == "LineHitBox")
            {
                Debug.Log("Raycast_Right");
                hit_x = hit_right.point.x;
                hit_y = hit_right.point.y;
                Point_Find();
            }

            //if the left raycast is closest to the line, store the hit point and start the function that finds where the hit is on the line
            if(hit_left.distance <= 3f && hit_right.distance > 3f && hit_left.collider.gameObject.tag == "LineHitBox")
            {
                Debug.Log("Raycast_Left");
                hit_x = hit_left.point.x;
                hit_y = hit_left.point.y;
                Point_Find();
            }

            //if the down raycast is closest to the line, store the hit point and start the function that finds where the hit is on the line
            if(hit_right.distance > 3f && hit_left.distance > 3f && hit_down.collider.gameObject.tag == "LineHitBox")
            {
                Debug.Log("Raycast_Down");
                hit_x = hit_down.point.x;
                hit_y = hit_down.point.y;
                Point_Find();
            }
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
                 //X CHECK FOR ASCENDING LINE: If y value check satisfied, determine whether the hit point x value is in between the x values of two consecutive line collider points (x and y check could be done in one if statement with && but it would get really long, like this comment LUL)
                if (line_collider_points[i-1].x <= hit_x && hit_x <= line_collider_points[i].x)
                {
                    //store the point of the line collider and start the line folowing function
                    I = i;
                    start_position = rb.transform.position; 
                    online = true;
                    startTime = Time.time;
                }
            }
            
            //Y CHECK FOR DESCENDING LINE: the check above won't work if [i-1].y > [i].y (i.e. line descending). Could do it all in one if statement with || but it would be mega long
            if (line_collider_points[i-1].y >= hit_y && hit_y >= line_collider_points[i].y)
            {
                //X CHECK FOR DESCENDING LINE
                if (line_collider_points[i-1].x >= hit_x && hit_x >= line_collider_points[i].x)
                {
                    //store the point of the line collider and start the line folowing function
                    I = i;
                    start_position = rb.transform.position;
                    online = true;
                    startTime = Time.time;
                }
            }
        }
    }
}