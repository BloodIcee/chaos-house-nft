using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField] private Vector2 referenceResolution;
    [SerializeField] [Range(0, 1)] private float widthToHeight;

    private new Camera camera;
    private float targetAspect;
    private float initial;
    private float horizontalFov;

    protected void Awake()
    {
        this.camera = this.GetComponent<Camera>();

        this.targetAspect = referenceResolution.x / referenceResolution.y;
        this.initial = camera.orthographic ? camera.orthographicSize : camera.fieldOfView;
        this.horizontalFov = CalcFov(camera.fieldOfView, 1 / targetAspect);
    }

    protected void Update()
    {
        if (camera.orthographic)
        {
            float widthSize = initial * (targetAspect / camera.aspect);
            camera.orthographicSize = Mathf.Lerp(widthSize, initial, widthToHeight);
        }
        else
        {
            float widthFov = CalcFov(horizontalFov, camera.aspect);
            camera.fieldOfView = Mathf.Lerp(widthFov, initial, widthToHeight);
        }
    }

    private float CalcFov(float horizotnalFovDeg, float aspectRation)
    {
        float horizotnalFovRad = horizotnalFovDeg * Mathf.Deg2Rad;
        float verticalFovRad = 2 * Mathf.Atan(Mathf.Tan(horizotnalFovRad / 2) / aspectRation);
        return verticalFovRad * Mathf.Rad2Deg;
    }

}
