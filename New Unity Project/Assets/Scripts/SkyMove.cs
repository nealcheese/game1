using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMove : MonoBehaviour
{

    public Transform sky;
    public Transform camera;
    float cloud = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        sky.position = new Vector3(sky.position.x, sky.position.y, sky.position.z);
       
    }

    // Update is called once per frame
    void Update()
    {
       
        sky.position = new Vector3(((camera.position.x * 0.85f) - cloud), sky.position.y, sky.position.z);
        cloud = cloud + 0.0005f;
        Debug.Log(cloud);
    }
}
