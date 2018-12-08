﻿using UnityEngine;

public class MasterCameraController : MonoBehaviour
{
    public static readonly float MIN_CAMERA_HEIGHT = 1f;

    public static readonly float MAX_CAMERA_HEIGHT = 60f;
    public static readonly float MAX_CAMERA_WIDTH = 120f;

    public GameObject ForgroundCamera;
    public GameObject BackgroundCamera;
    public GameObject Subject;

    private readonly float ZoomSpeed = 1.2f;

    void FixedUpdate ()
    {
        ForgroundCamera.transform.position = new Vector3(Subject.transform.position.x, Subject.transform.position.y, ForgroundCamera.transform.position.z);
        BackgroundCamera.transform.position = new Vector3(Subject.transform.position.x, Subject.transform.position.y, BackgroundCamera.transform.position.z);

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
            else if (BackgroundCamera.GetComponent<Camera>().orthographicSize < 1f)
            {
                BackgroundCamera.GetComponent<Camera>().orthographicSize = 1f;
                ForgroundCamera.GetComponent<Camera>().orthographicSize = 1f;
            }
        }
    }
}
