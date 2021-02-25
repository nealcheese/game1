using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHitbox : MonoBehaviour
{

    public Rigidbody2D rb;
    public GameObject LineHitBox;
    public Collider2D platform;
    private float timer = 0.0f;
    Vector3 hit;
    Vector2 hit_Tangent2;
    private bool using_wall = false;

    void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }
  
    void Update()
    {
        timer += Time.deltaTime; 
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        using_wall = false;
        

        if (collider.gameObject.tag == "LineHitBox")
        {

            Vector3 raycast_right_start = new Vector3 (rb.transform.position.x, rb.transform.position.y, rb.transform.position.z);
            Vector3 raycast_left_start = new Vector3 (rb.transform.position.x, rb.transform.position.y, rb.transform.position.z);
            Vector3 raycast_down_start = new Vector3 (rb.transform.position.x, rb.transform.position.y+2f, rb.transform.position.z);

            RaycastHit2D hit_down = Physics2D.Raycast(raycast_down_start, Vector2.down);
            RaycastHit2D hit_right = Physics2D.Raycast(raycast_right_start, Vector2.right);
            RaycastHit2D hit_left = Physics2D.Raycast(raycast_left_start, Vector2.left);
         
            
            
            EdgeCollider2D line_collider = LineHitBox.GetComponent<EdgeCollider2D>();
            Vector2[] line_collider_points = line_collider.points;

            Vector3 z_Cross = new Vector3(0.0f,0.0f,3.0f);


            if(hit_right.distance <= 1.5f && hit_left.distance > 1.5f)
            {
               Debug.Log("collision_RIGHT"); 
                
                hit = new Vector3(hit_right.normal.x, hit_right.normal.y, 0.0f);
                Vector3 hit_Tangent3 = Vector3.Cross(hit, z_Cross);

                for(int i = 1; i < line_collider_points.Length; i++)
                {
                    if (line_collider_points[i-1].y <= hit_right.point.y && hit_down.point.y <= line_collider_points[i].y)
                    {

                       
                    
                            hit_Tangent2 = new Vector2(hit_Tangent3.x*2, hit_Tangent3.y);

                    }
                    

                    if (line_collider_points[i-1].y >= hit_right.point.y && hit_right.point.y >= line_collider_points[i].y)
                    {

                            hit_Tangent2 = new Vector2(-hit_Tangent3.x*2, -hit_Tangent3.y);

                    }
                }

            } 

            if(hit_left.distance <= 1.5f && hit_right.distance > 1.5f)
            {

                Debug.Log("collision_LEFT");

                
                hit = new Vector3(hit_left.normal.x, hit_left.normal.y, 0.0f);
                Vector3 hit_Tangent3 = Vector3.Cross(hit, z_Cross);

                for(int i = 1; i < line_collider_points.Length; i++)
                {
                    if (line_collider_points[i-1].y <= hit_left.point.y && hit_left.point.y <= line_collider_points[i].y)
                    {

                            hit_Tangent2 = new Vector2(hit_Tangent3.x, -hit_Tangent3.y);


                    }

                    if (line_collider_points[i-1].y >= hit_left.point.y && hit_left.point.y >= line_collider_points[i].y)
                    {

                            hit_Tangent2 = new Vector2(hit_Tangent3.x, hit_Tangent3.y);

                    }
                }

                

            } 




           
           
           if(hit_left.distance > 1.5f &&  hit_right.distance > 1.5f)
            {

                Debug.Log("collision_down");
                hit = new Vector3(hit_down.normal.x, hit_down.normal.y, 0.0f);
                Vector3 hit_Tangent3 = Vector3.Cross(hit, z_Cross);
                
                for(int i = 1; i < line_collider.points.Length; i++)
                {
                    if (line_collider_points[i-1].x <= hit_down.point.x && hit_down.point.x <= line_collider_points[i].x)
                    {

                            hit_Tangent2 = new Vector2(hit_Tangent3.x, hit_Tangent3.y);
                        
                    }

                    if (line_collider_points[i-1].x >= hit_down.point.x && hit_down.point.x >= line_collider_points[i].x)
                    {
                            hit_Tangent2 = new Vector2(-hit_Tangent3.x, -hit_Tangent3.y);
                            
                    }
                    
                }

                
            }
            
            
            

        
            rb.velocity = hit_Tangent2*8;
            timer = 0.0f;
        }

    }

}