using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSensitivity = 15.0f;
    public float zoomSpeed = 5.0f;
    public float zoomEndFOV = 26f;
    public float zoomMax = 80.0f;

    private float zoom;


    void Start()
    {
        zoom = Camera.main.fieldOfView;
    }



    void LateUpdate()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomEndFOV, Time.deltaTime * zoomSpeed);
    }
}
