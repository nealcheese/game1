using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Character;
    public Camera cam;
    

 

    void Update()
    {

        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        if (Character.position.x > (cam.transform.position.x - ((width/2)*0.1)))
        {
            cam.transform.position = new Vector3(cam.transform.position.x + (width/400), cam.transform.position.y, cam.transform.position.z);

        }
         
        if (Character.position.x < (cam.transform.position.x - ((width / 2) * 0.3)))
        {
            cam.transform.position = new Vector3(cam.transform.position.x - (width / 400), cam.transform.position.y, cam.transform.position.z);

        }

        if (Character.position.y > (cam.transform.position.y + ((height / 2) * 0.3)))
        {
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + (height / 200), cam.transform.position.z);

        }

        if (Character.position.y < (cam.transform.position.y - ((height / 2) * 0.9)))
        {
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - (height / 200), cam.transform.position.z);

        }
    }
}
 