﻿using UnityEngine;

public class MasterCameraController : MonoBehaviour
{
    public GameObject ForgroundCamera;
    public GameObject BackgroundCamera;

    private readonly float PanSpeed = 10f;
    private readonly float ZoomSpeed = 1.2f;

    void Update ()
    {

        Vector3 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position += movement * Time.deltaTime * PanSpeed;

        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            BackgroundCamera.GetComponent<Camera>().orthographicSize *= (1f / ZoomSpeed);
            ForgroundCamera.GetComponent<Camera>().orthographicSize *= (1f / ZoomSpeed);
        }
        else if (d < 0f)
        {
            BackgroundCamera.GetComponent<Camera>().orthographicSize *= ZoomSpeed;
            ForgroundCamera.GetComponent<Camera>().orthographicSize *= ZoomSpeed;
            if (BackgroundCamera.GetComponent<Camera>().orthographicSize > 30f )
            {
                BackgroundCamera.GetComponent<Camera>().orthographicSize = 30f;
                ForgroundCamera.GetComponent<Camera>().orthographicSize = 30f;
            }
        }
    }
}
