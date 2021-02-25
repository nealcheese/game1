using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treeground : MonoBehaviour
{
    public Transform camera;
    public Transform background;

    void Start()
    {
        background.position = new Vector3(background.position.x, background.position.y, background.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        background.position = new Vector3((camera.position.x * 0.7f), background.position.y, background.position.z);
    }
}
