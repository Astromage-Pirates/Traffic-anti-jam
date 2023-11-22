using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtDirection : MonoBehaviour
{
    private Camera myCamera;

    private void Start()
    {
        myCamera = Camera.main;
    }
    void Update()
    {
        Camera camera = Camera.main;
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }
}
