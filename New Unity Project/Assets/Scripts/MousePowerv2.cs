using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MousePowerv2 : MonoBehaviour
{
    
    List<Vector3> linePoints = new List<Vector3>();
    List<Vector2> linePoints_v2 = new List<Vector2>();
    List<Vector3> BezierPoints3 = new List<Vector3>();
    List<Vector2> BezierPoints2 = new List<Vector2>();
    List<int> NewLineStart = new List<int>();
    List<LineRenderer> LineList = new List<LineRenderer>();
    List<GameObject> LineObjects = new List<GameObject>();
    List<EdgeCollider2D> ColliderList = new List<EdgeCollider2D>();
    List<EdgeCollider2D> ColliderList2 = new List<EdgeCollider2D>();
    List<GameObject> ColliderObjects = new List<GameObject>();
    List<GameObject> ColliderObjects2 = new List<GameObject>();
    List<GameObject> WritingPages = new List<GameObject>();

    LineRenderer lineRenderer;
    private LineRenderer lineRendererinitial;
    public EdgeCollider2D polyCollider;
    private EdgeCollider2D polyColliderinitial;
    public EdgeCollider2D polyCollider2;
    private EdgeCollider2D polyCollider2initial;
    public CompositeCollider2D tilemap;

    public float startWidth = 0.1f;
    public float endWidth = 0.1f;
    public float threshold = 0.001f;
    Camera thisCamera;
    float Cooldown_timer = 0.0f;
    float Casting_timer = 0.0f;
    public float cooldown = 3.0f;
    public float casting_time = 0.5f;
    bool oncooldown = false;
    bool casting = false;
    Vector2[] BezierCorrected;
    int stopdrawinghere;
    bool earlystop;
    bool inwall;
    public GameObject LineHitBox;
    public GameObject LineFollow;
    public GameObject WritingPageSprite;
    GameObject lineclone;
    GameObject colliderclone;
    GameObject colliderclone2;
    GameObject writingpageclone;
    Vector3[] WipeLinePoints;
    Vector2[] WipeColliderPoints;
    Vector2[] WipeColliderPoints2;
    public Sprite Page_A;
    public Sprite Page_B;
    public Sprite Page_C;
    public float page_charge;
    public TextMeshProUGUI page_charge_display;
    List<Sprite> Page_list = new List<Sprite>();

    float draw_distance;

    float distance_sum;
    bool summing_distance;

    Vector3 lastPos = Vector3.one * float.MaxValue;

    void Awake()
    {
        //find camera and linerenderer component on this object
        thisCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(0);
        Physics2D.queriesStartInColliders = false;
        
        polyColliderinitial = polyCollider;
        polyCollider2initial = polyCollider2;
        lineRendererinitial = lineRenderer;

        Page_list.Add(Page_A);
        Page_list.Add(Page_B);
        Page_list.Add(Page_C);
        
    }

    void Update()
    {
        
        if (oncooldown == true)
        {
            Cooldown_timer += Time.deltaTime;
            
        }
        
        if (casting == true)
        {
            Casting_timer += Time.deltaTime;
            
        }

        if (oncooldown == false)
        {
            Cooldown_timer = 0.0f;
            
        }
        
        if (casting == false)
        {
            
            Casting_timer = 0.0f;
        }

        //page_charge counter
        page_charge_display.text = "text";

        if (Input.GetMouseButtonDown(0) && oncooldown == false && casting == false) 
        {
            //casting started
            casting = true;
            earlystop = false;
            //Debug.Log("castingStart");
            polyColliderinitial.enabled = true;
            polyCollider2initial.enabled = true;
            draw_distance = 0f;
            StopAllCoroutines();
        }

        if(Input.GetMouseButton(0) && casting == true) 
        {
           
            
            //store mouse points while holding down button and not on cooldown
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorld = thisCamera.ScreenToWorldPoint(mousePos);
            Vector2 mouseWorld_v2 = thisCamera.ScreenToWorldPoint(mousePos);
            mouseWorld.z = -2.46f;

            
            if (linePoints.Count > 1)
            {
                draw_distance = draw_distance + Vector3.Distance(linePoints[linePoints.Count - 1], mouseWorld);
                Debug.Log(draw_distance);
            }

            lastPos = mouseWorld;
                if (linePoints == null)
                    linePoints = new List<Vector3>();

            linePoints.Add(mouseWorld);
            linePoints_v2.Add(mouseWorld_v2);
            casting = true;
            //Debug.Log("casting");
            


        }

        if (oncooldown == false && casting == true)
        {
                if (Input.GetMouseButtonUp(0) || draw_distance >= 10f)
                {
                    //casting finishes when mouse button lifted or casting time up
                    oncooldown = true;
                    casting = false;
                    //Debug.Log("CastingOver");
                    
                    BezierTime(); 
                } 
        }

        if (Cooldown_timer >= cooldown && oncooldown == true && casting == false)
        {
            //cooldown ends
            //Debug.Log("cooldownOver");
            oncooldown = false;

        }
    } 

    void BezierTime()
    {
        //Debug.Log("BezierStart");
        int i;
        NewLineStart.Add(0);
        //Debug.Log(linePoints.Count);
        for (i = 0; i < linePoints.Count - 2; i+=2)
        {

            
            //BezierPoints3.Add(linePoints[i]);
            
            for (float t = 0.05f; t < 1f; t=t+0.15f)
            {

                //Debug.Log(linePoints[i].x);
                //Debug.Log(linePoints[i+1].x);
                //Debug.Log(linePoints[i+2].x);
                
                float x = (Mathf.Pow((1-t),2)*linePoints[i].x) + (2*(1-t)*t*linePoints[i+1].x) + (Mathf.Pow(t,2)*linePoints[i+2].x);
                float y = (Mathf.Pow((1-t),2)*linePoints[i].y) + (2*(1-t)*t*linePoints[i+1].y) + (Mathf.Pow(t,2)*linePoints[i+2].y);

                Vector3 Bezier3 = new Vector3 (x, y, -2.46f);
                Vector2 Bezier2 = new Vector3 (x, y);

                RaycastHit2D col_up = Physics2D.Raycast(Bezier2, new Vector2(0f,1f), Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"));
                RaycastHit2D col_down = Physics2D.Raycast(Bezier2, new Vector2(0f,-1f), Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"));
                RaycastHit2D col_right = Physics2D.Raycast(Bezier2, new Vector2(1f,0f), Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"));
                RaycastHit2D col_left = Physics2D.Raycast(Bezier2, new Vector2(-1f,0f), Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"));
                
                if(col_up.distance > 0f && col_down.distance > 0f && col_right.distance > 0f && col_left.distance > 0f)
                {
                    if(BezierPoints2.Count == 0)
                    {
                        //Debug.Log("Started in Ground");
                        //earlystop = true;
                    }

                    if(BezierPoints2.Count > 0 && inwall == false)
                    {
                        //Debug.Log("Into Ground");
                        inwall = true;
                        //NewLineStart.Add(BezierPoints2.Count);
                        //Debug.Log(NewLineStart.Count);
                    }

                    if(BezierPoints2.Count > 0 && inwall == true)
                    {

                    }

                } else 
                {
                    if(inwall == true)
                    {
                        //Debug.Log("Out of Ground");
                        inwall = false;
                        NewLineStart.Add(BezierPoints2.Count);
                        //Debug.Log(NewLineStart.Count);
                        
                    }
                    
                    if(inwall == false)
                    {
                        BezierPoints3.Add(Bezier3);
                        BezierPoints2.Add(Bezier2);
                    }
                }

                
                    

                

            }
            

        } 
        
        if (i > (linePoints.Count - 4))
        {
            //Debug.Log(i);
            //Debug.Log("Coroutines");
            NewLineStart.Add(BezierPoints2.Count);
            StartCoroutine(UpdateLine());
            StartCoroutine(UpdateCollider());
            return;
            
        }

    }

    IEnumerator UpdateLine()
    {
        lineRenderer = lineRendererinitial;
        yield return new WaitForSeconds(0.0001f);
        lineRenderer.SetWidth(startWidth, endWidth);
        writingpageclone = Instantiate(WritingPageSprite, BezierPoints3[0], Quaternion.identity);
        WritingPages.Add(writingpageclone);
        distance_sum = 0.0f;
        summing_distance = false;
        //need a list to store the writing page objects, can then check distance of a bezier points against the last writing page object
        for (int j = 0; j <NewLineStart.Count-1; j++)
        { 
            //Debug.Log(NewLineStart[j]);
            for (int i = NewLineStart[j]; i < NewLineStart[j+1]; i++)
            {
        
                //Debug.Log("LineDraw");
                lineRenderer.SetVertexCount(i - NewLineStart[j] + 1);
                lineRenderer.SetPosition(i- NewLineStart[j], BezierPoints3[i]);
                
                if(distance_sum >= 0.5f)
                {
                    writingpageclone = Instantiate(WritingPageSprite, BezierPoints3[i], Quaternion.Euler(0f,0f, Random.Range(0f, 359f)));
                    SpriteRenderer pagesprite = writingpageclone.GetComponent<SpriteRenderer>();
                    pagesprite.sprite = Page_list[(int)Random.Range(0f, 3f)];
                    //pagesprite.sprite = Page_list[1];
                    WritingPages.Add(writingpageclone);
                    summing_distance = false;
                    distance_sum = 0.0f;
                } else
                {
                    if(summing_distance == false)
                    {
                        distance_sum = distance_sum + Vector3.Distance(WritingPages[WritingPages.Count - 1].transform.position, BezierPoints3[i]);
                        summing_distance = true;
                    }

                    if(summing_distance == true && i > 0)
                    {
                        distance_sum = distance_sum + Vector3.Distance(BezierPoints3[i-1], BezierPoints3[i]);
                        //Debug.Log(distance_sum);
                    }
                }
                yield return new WaitForSeconds(0.00005f);
                

            }

            LineList.Add(lineRenderer);

            if(j<NewLineStart.Count-2)
            {
                lineclone = Instantiate(gameObject);
                LineObjects.Add(lineclone);
                lineRenderer = lineclone.GetComponent<LineRenderer>();
            }
        }
        
        StartCoroutine(WipeLine());
        
    }

    IEnumerator WipeLine()
    {
        //Debug.Log(LineList[0].positionCount);
        int m = 0;

        
        for(int j = 0; j < LineList.Count; j++)
        {
            //Debug.Log("WipeLine");
            lineRenderer = LineList[j];
            WipeLinePoints = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(WipeLinePoints);
            
            for (int k = 1; k < WipeLinePoints.Length; k++)
            {
                for(int i = k; i < WipeLinePoints.Length; i++)
                {
                    lineRenderer.SetVertexCount(WipeLinePoints.Length - k);
                    lineRenderer.SetPosition(i - k, WipeLinePoints[i]);





                }

                if(m < WritingPages.Count)
                {
                    float distance = Mathf.Abs(Vector3.Distance(WipeLinePoints[k], WritingPages[m].transform.position));
                    //Debug.Log(distance);
                    if(distance < 0.1f)
                    {
                        Debug.Log("WritingDelete");
                        Destroy(WritingPages[m]);
                        m=m+1;
                    }
                }
                yield return new WaitForSeconds(0.00005f);
                //Debug.Log("WipeLine");
            }

            

        }

        for(int j = 0; j < LineObjects.Count; j++)
        {
           Destroy(LineObjects[j]);
        }
        
        LineList.Clear();
        linePoints.Clear();
        BezierPoints3.Clear();
        NewLineStart.Clear();
        LineObjects.Clear();
        WritingPages.Clear();
        
        
    }

    IEnumerator UpdateCollider()
    {
        polyCollider = polyColliderinitial;
        yield return new WaitForSeconds(0.00005f);

        LineCorrect();
        
        
        List<Vector2> BezierPoints2_b = new List<Vector2>();
        BezierPoints2_b.Add(BezierPoints2[0]);



        for (int j = 0; j <NewLineStart.Count-1; j++)
        {

            for (int i = NewLineStart[j]; i < NewLineStart[j+1]; i++)
            {
                BezierPoints2_b.Add(BezierPoints2[i]);
                polyCollider.points = BezierPoints2_b.ToArray();
                yield return new WaitForSeconds(0.00005f);
            }

            BezierPoints2_b.Clear();
            ColliderList.Add(polyCollider);

            if(j<NewLineStart.Count-2)
            {
                colliderclone = Instantiate(LineHitBox);
                ColliderObjects.Add(colliderclone);
                polyCollider = colliderclone.GetComponent<EdgeCollider2D>();
            }
        }

        StartCoroutine(WipeCollider());
    }
    IEnumerator WipeCollider()
    {

        for(int j = 0; j < ColliderList.Count; j++)
        {
            Debug.Log("WipeCollider");
            polyCollider = ColliderList[j];
            polyCollider2 = ColliderList2[j];
            WipeColliderPoints = new Vector2[polyCollider.pointCount];
            WipeColliderPoints = polyCollider.points;
            WipeColliderPoints2 = new Vector2[polyCollider2.pointCount];
            WipeColliderPoints2 = polyCollider2.points;

            List<Vector2> BezierPoints2_b = new List<Vector2>();
            List<Vector2> BezierCorrected_b = new List<Vector2>();
            
            for (int k = 1; k < WipeColliderPoints.Length; k++)
            {
                for(int l = k; l < WipeColliderPoints.Length; l++)
                {
                    BezierPoints2_b.Add(WipeColliderPoints[l]);
                    BezierCorrected_b.Add(WipeColliderPoints2[l]);
                    
                }
                polyCollider.points = BezierPoints2_b.ToArray();
                polyCollider2.points = BezierCorrected_b.ToArray();
                BezierPoints2_b.Clear();
                BezierCorrected_b.Clear();
                yield return new WaitForSeconds(0.00005f);
            }

            polyColliderinitial.enabled = false;
            polyCollider2initial.enabled = false;
            
        }

        for(int j = 0; j < ColliderObjects.Count; j++)
        {
           Destroy(ColliderObjects[j]);
           Destroy(ColliderObjects2[j]);

        }

        BezierPoints2.Clear();
        linePoints_v2.Clear();
        ColliderList.Clear();
        ColliderObjects.Clear();
        ColliderList2.Clear();
        ColliderObjects2.Clear();
            
        
    }

    void LineCorrect()
    {
        BezierCorrected = BezierPoints2.ToArray();
        //Debug.Log(BezierCorrected.Length);
        //Debug.Log("LineCorrect");
        
        for (int i = 0; i < BezierCorrected.Length; i++)
        {
            //Debug.Log("LineCorrect");
            RaycastHit2D col_down = Physics2D.Raycast(BezierCorrected[i], new Vector2(0f,-1f), Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"));
            RaycastHit2D col_right = Physics2D.Raycast(BezierCorrected[i], new Vector2(1f,0f), Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"));
            RaycastHit2D col_left = Physics2D.Raycast(BezierCorrected[i], new Vector2(-1f,0f), Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"));
            //Debug.Log(col_down.distance);
            if(col_right.distance <=0.5f)
            {
                BezierCorrected[i] = new Vector2 (BezierCorrected[i].x - (0.5f - col_right.distance), BezierCorrected[i].y);
                //Debug.Log("close to right");
            }

            if(col_left.distance <=0.5f)
            {
                BezierCorrected[i] = new Vector2 (BezierCorrected[i].x + (0.5f - col_left.distance), BezierCorrected[i].y);
                //Debug.Log("close to right");
            }

            if(col_down.distance <1.5f)
            {
                BezierCorrected[i] = new Vector2 (BezierCorrected[i].x , BezierCorrected[i].y + (1.5f - col_down.distance));
                //Debug.Log("close to ground");
                //Debug.Log(col_down.distance);
            }


        }

        List<Vector2> BezierCorrected_b = new List<Vector2>();
        BezierCorrected_b.Add(BezierCorrected[0]);
        polyCollider2 = polyCollider2initial;



        for (int j = 0; j <NewLineStart.Count-1; j++)
        {

            for (int i = NewLineStart[j]; i < NewLineStart[j+1]; i++)
            {
                BezierCorrected_b.Add(BezierCorrected[i]);
                polyCollider2.points = BezierCorrected_b.ToArray();
            }

            BezierCorrected_b.Clear();
            ColliderList2.Add(polyCollider2);
            if(j<NewLineStart.Count-2)
            {
                colliderclone2 = Instantiate(LineFollow);
                ColliderObjects2.Add(colliderclone2);
                polyCollider2 = colliderclone2.GetComponent<EdgeCollider2D>();
            }
            
        }
    }
}
