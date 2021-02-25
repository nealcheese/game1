using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MousePower : MonoBehaviour
{
    List<Vector3> linePoints = new List<Vector3>();
    List<Vector2> linePoints_v2 = new List<Vector2>();
    List<Vector3> BezierPoints3 = new List<Vector3>();
    List<Vector2> BezierPoints2 = new List<Vector2>();

    LineRenderer lineRenderer;
    public EdgeCollider2D polyCollider;
    

    public float startWidth = 0.5f;
    public float endWidth = 0.5f;
    public float threshold = 0.001f;
    Camera thisCamera;
    float timer = 3.0f;
    float timer2 = 3.0f;
    public float cooldown = 3.0f;
    public float casting_time = 1.0f;
    bool oncooldown = false;
    bool casting = false;
    bool BezierDone = false;



    Vector3 lastPos = Vector3.one * float.MaxValue;


    void Awake()
    {
        thisCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        //polyCollider = gameObject.AddComponent(typeof(PolygonCollider2D)) as PolygonCollider2D;
        lineRenderer.SetVertexCount(0);
    }

    void Update()
    {

        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        Debug.Log(timer);

        if (Input.GetMouseButtonDown(0) && oncooldown == false) 
        {

            //Debug.Log("Button_Pressed");
            
            timer2 = 0.0f;
            polyCollider.enabled = true;


        }

        while(Input.GetMouseButton(0) && oncooldown == false && timer2 < casting_time) 
        {

                Vector3 mousePos = Input.mousePosition;
                Vector3 mouseWorld = thisCamera.ScreenToWorldPoint(mousePos);
                Vector2 mouseWorld_v2 = thisCamera.ScreenToWorldPoint(mousePos);
                mouseWorld.z = -2.46f;

                float dist = Vector3.Distance(lastPos, mouseWorld);
                if (dist <= threshold)
                    return;

                lastPos = mouseWorld;
                if (linePoints == null)
                    linePoints = new List<Vector3>();

               
                linePoints.Add(mouseWorld);
                linePoints_v2.Add(mouseWorld_v2);
                casting = true;
            
        } 

        if (oncooldown == false && casting == true)
        {
            if (Input.GetMouseButtonUp(0) || timer2 >= casting_time)

            {
                
                timer = 0.0f;
                Debug.Log("BezierTime");
                oncooldown = true;
                casting = false;
                
                
                BezierTime();
                
                
                
            }
        }

        if (timer >= cooldown && oncooldown == true && casting == false)
        {
            Debug.Log("cooldownOver");
            oncooldown = false;

            return;
        }
    }


    void BezierTime()
    {
        
        for (int i = 0; i < linePoints.Count - 2; i+=2)
        {
            //BezierPoints3.Add(linePoints[i]);
            
            for (float t = 0.1f; t < 1f; t=t+0.1f)
            {
                
                //Debug.Log(linePoints[i].x);
                //Debug.Log(linePoints[i+1].x);
                //Debug.Log(linePoints[i+2].x);
                
                float x = (Mathf.Pow((1-t),2)*linePoints[i].x) + (2*(1-t)*t*linePoints[i+1].x) + (Mathf.Pow(t,2)*linePoints[i+2].x);
                float y = (Mathf.Pow((1-t),2)*linePoints[i].y) + (2*(1-t)*t*linePoints[i+1].y) + (Mathf.Pow(t,2)*linePoints[i+2].y);

                Vector3 Bezier3 = new Vector3 (x, y, -2.46f);
                Vector2 Bezier2 = new Vector3 (x, y);
                BezierPoints3.Add(Bezier3);
                BezierPoints2.Add(Bezier2);

            }
            
            if (i == linePoints.Count - 3)
            {
                    StartCoroutine(UpdateLine());
                    StartCoroutine(UpdateCollider());
                    return;
                
            }
        } 

    }



    IEnumerator UpdateLine()
    {
        
        yield return new WaitForSeconds(0.001f);
        lineRenderer.SetWidth(startWidth, endWidth);



        for (int i = 0; i < BezierPoints3.Count - 1; i++)
        {
            timer = 0.0f;
            lineRenderer.SetVertexCount(i + 1);
            lineRenderer.SetPosition(i, BezierPoints3[i]);
            yield return new WaitForSeconds(0.001f);
            

        }

        StartCoroutine(WipeLine());
    }

    IEnumerator WipeLine()
    {
        
        for (int k = 1; k < BezierPoints3.Count; k++)
        {
            for(int i = k; i < BezierPoints3.Count; i++)
            {
                lineRenderer.SetVertexCount(BezierPoints3.Count - k);
                lineRenderer.SetPosition(i - k, BezierPoints3[i]);
               timer = 0.0f; 
            }
           yield return new WaitForSeconds(0.001f);
           //Debug.Log("WipeLine");
        }

        linePoints.Clear();
        BezierPoints3.Clear();
    }

    IEnumerator UpdateCollider()
    {
        yield return new WaitForSeconds(0.001f);
        Vector2[] colliderpoints = polyCollider.points;
        List<Vector2> BezierPoints2_b = new List<Vector2>();
        BezierPoints2_b.Add(BezierPoints2[0]);

        for (int j = 1; j < BezierPoints2.Count - 1; j++)
        {
            BezierPoints2_b.Add(BezierPoints2[j]);
            polyCollider.points = BezierPoints2_b.ToArray();
            yield return new WaitForSeconds(0.001f);
        }

        StartCoroutine(WipeCollider());
    }

    IEnumerator WipeCollider()
    {

        Vector2[] colliderpoints = polyCollider.points;
        List<Vector2> BezierPoints2_b = new List<Vector2>();
        
        for (int k = 1; k < BezierPoints2.Count; k++)
        {
            for(int j = k; j < BezierPoints2.Count; j++)
            {
                BezierPoints2_b.Add(BezierPoints2[j]);

                
            }
            polyCollider.points = BezierPoints2_b.ToArray();
            BezierPoints2_b.Clear();
            yield return new WaitForSeconds(0.001f);
        }

        BezierPoints2.Clear();
        linePoints_v2.Clear();
        polyCollider.enabled = false;
        
    }
}