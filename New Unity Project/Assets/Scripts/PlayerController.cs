//Create a new 2D Sprite GameObject and attach this script to it.

//This script moves a GameObject up or down when you press the up or down arrow keys.
//The velocity is set to the Vector2() value.  Unchanging the Vector2() keeps the
//GameObject moving at a constant rate.

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    

    private bool moving = false;
    private bool strafing = false;
    private bool air_strafe = false;
    private bool onground = true;
    private bool onhilldown = false;
    private bool onhillup = false;
    private bool jumping = false;

    public int drag_hill = 1;
    public int drag_flat = 4;
    public int drag_jump = 1;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetTrigger("Idle");
       
    }

    void OnCollisionStay2D(Collision2D collision)
    {

        onground = false;
        onhilldown = false;
        onhillup = false;
        air_strafe = false;
        jumping = false;
        

        foreach (ContactPoint2D cPoint in collision.contacts)
        {
            if (cPoint.normal.y > 0.99)
            {
                onground = true;
                //Debug.Log("onground");

  
                break;
            }

            if (cPoint.normal.x > 0.65 && cPoint.normal.x < 0.75)
            {
                onground = true;
                onhilldown = true;
                //Debug.Log("onhilldown");


                break;
            }

            if (cPoint.normal.x > -0.75 && cPoint.normal.x < -0.65)
            {
                onground = true;
                onhillup = true;
                //Debug.Log("onhillup");

                break;
            }


            continue;


        }
    }

    void OnCollisionEnter2D()
    {
        float x_Velocity = rb.velocity.x;
        //Debug.Log("LandedEnter");

        if (jumping == true && x_Velocity == 0.0f)
        {

            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("JumpingLeft");
            anim.SetTrigger("Idle");

        }

        if (jumping == true && x_Velocity >= 0.0001f)
        {
            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("JumpingLeft");
            anim.SetTrigger("RunReturn");

        }

        if (jumping == true && x_Velocity <= -0.0001f)
        {
            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("JumpingLeft");
            anim.SetTrigger("RunReturnLeft");

            
        }
    }

    void OnCollisionExit2D()
    {
        onground = false;
        onhilldown = false;
        onhillup = false;
        air_strafe = false;
        jumping = true;
        //Debug.Log("jumpingExit");
    }

    void Update()
    {

        
 
            //Press the Up arrow key to move the RigidBody upwards
            if (Input.GetKey(KeyCode.W) && (onground == true || onhilldown == true || onhillup == true))
            {
                 
                rb.drag = drag_jump;
                
                 float x_Velocity = rb.velocity.x;
                double x_VelocityDouble = x_Velocity * 1.2;
                    x_Velocity = (float)x_VelocityDouble;

                        rb.velocity = new Vector2(x_Velocity, 25.0f);
                        moving = true;
               
                        onground = false;
                      jumping = true;

                    anim.ResetTrigger("Idle");
                    anim.ResetTrigger("Running");
                    anim.ResetTrigger("RunningLeft");
                    anim.ResetTrigger("RunReturn");
                    anim.ResetTrigger("RunReturnLeft");

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && x_Velocity >=0.0f)
                    {
                anim.SetTrigger("StationaryJump");
                    }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && x_Velocity < 0.0f)
            {
                anim.SetTrigger("StationaryJumpLeft");
                    }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
                anim.SetTrigger("Jumping");
                    }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("RunningLeft"))
            {
                anim.SetTrigger("JumpingLeft");
                    }

        }

        //Press the Down arrow key to move the RigidBody downwards
        if (Input.GetKey(KeyCode.S))
        {

            rb.velocity = new Vector2(0.0f, 4.0f);
            strafing = true;

            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("JumpingLeft");
            anim.SetTrigger("Running");


        }

        //Press the RIGHT arrow key to move the RigidBody RIGHT on flatground
        if (Input.GetKey(KeyCode.D) && onground == true && jumping == false && onhilldown == false && onhillup == false)
        {

            rb.drag = drag_flat;

            rb.velocity = new Vector2(10f, 0f);
            moving = true;

            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("JumpingLeft");
            anim.SetTrigger("Running");

        }

        //Press the RIGHT arrow key to move the RigidBody RIGHT on hilldown
        if (Input.GetKey(KeyCode.D) && onhilldown == true && onhillup == false)
        {

           

            rb.velocity = new Vector2(4.0f, -4.0f);
            moving = true;

            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("JumpingLeft");
            anim.SetTrigger("Running");

        }

        //Press the RIGHT arrow key to move the RigidBody RIGHT on hillup
        if (Input.GetKey(KeyCode.D) && onhilldown == false && onhillup == true)
        {

            rb.drag = drag_hill;

            rb.velocity = new Vector2(6.0f, 6.0f);
            moving = true;

            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("JumpingLeft");
            anim.SetTrigger("Running");

        }


        //Press the LEFT arrow key to move the RigidBody LEFT on flatground
        if (Input.GetKey(KeyCode.A) && onground == true && jumping == false && onhilldown == false && onhillup == false)
        {
            rb.drag = drag_flat;

            rb.velocity = new Vector2(-10f, 0f);
            strafing = true;


            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("JumpingLeft");
            anim.SetTrigger("RunningLeft");

        }

        //Press the LEFT arrow key to move the RigidBody LEFT on hilldown
        if (Input.GetKey(KeyCode.A) && onhilldown == true && onhillup == false)
        {
            

            rb.velocity = new Vector2(-6.0f, 6.0f);
            strafing = true;


            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("JumpingLeft");
            anim.SetTrigger("RunningLeft");

        }

        //Press the LEFT arrow key to move the RigidBody LEFT on hillup
        if (Input.GetKey(KeyCode.A) && onhilldown == false && onhillup == true)
        {
            rb.drag = drag_hill;

            rb.velocity = new Vector2(-4.0f, -4.0f);
            strafing = true;

            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("JumpingLeft");
            anim.SetTrigger("RunningLeft");

        }

        //Stand still when no left or right input
        if (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false && jumping == false)
        {

            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("JumpingLeft");
            anim.SetTrigger("Idle");

        }
        //Move right a bit when jumping if you press right
        if (onground == false && air_strafe == false && Input.GetKey(KeyCode.D) == true)
        {

            float y_Velocity = rb.velocity.y;
            float x_Velocity = rb.velocity.x;

            if (x_Velocity < 4.0f)
            {

                rb.velocity = new Vector2((x_Velocity*0.8f + 4.0f), y_Velocity);
                air_strafe = false;

                anim.ResetTrigger("Jumping");
                anim.ResetTrigger("JumpingLeft");
                anim.SetTrigger("Idle");
            }

        }
        //Move left a bit when jumping if you press left
        if (onground == false && air_strafe == false && Input.GetKey(KeyCode.A) == true)
        {

            float y_Velocity = rb.velocity.y;
            float x_Velocity = rb.velocity.x;

            if (x_Velocity > -4.0f)
            {

                rb.velocity = new Vector2((x_Velocity*0.8f - 4.0f), y_Velocity);
                air_strafe = false;

                anim.ResetTrigger("Jumping");
                anim.ResetTrigger("JumpingLeft");
                anim.SetTrigger("Idle");
            }

        }



    }



}