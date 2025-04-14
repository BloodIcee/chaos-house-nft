using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSurviveMode : MonoBehaviour
{

    public Transform APRRoot;
    public float distance = 10.0f; 
    public float smoothness = 0.15f;
    public float maxDistance = 10f;

    private Camera cam;
    private Vector3 offset;

    private void Start()
    {
        cam = Camera.main;
        offset = cam.transform.position;
    }

    private void Update()
    {
        var direction = APRRoot.position + offset* distance/maxDistance;
        cam.transform.position = Vector3.Lerp(cam.transform.position, direction, smoothness);        
    }
}
