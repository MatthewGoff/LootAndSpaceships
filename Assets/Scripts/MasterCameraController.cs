using UnityEngine;

public class MasterCameraController : MonoBehaviour
{
    public static readonly float MIN_CAMERA_HEIGHT = 1f;
    public static readonly float MAX_CAMERA_HEIGHT = 60f;
    public static readonly float MAX_CAMERA_WIDTH = 120f;

    public static MasterCameraController Instance;

    public GameObject ForgroundCamera;
    public GameObject BackgroundCamera;
    public ITargetable Subject;

    private readonly float ZoomSpeed = 1.2f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update ()
    {
        ForgroundCamera.transform.position = new Vector3(Subject.GetPosition().x, Subject.GetPosition().y, ForgroundCamera.transform.position.z);
        BackgroundCamera.transform.position = new Vector3(Subject.GetPosition().x, Subject.GetPosition().y, BackgroundCamera.transform.position.z);

        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            BackgroundCamera.GetComponent<Camera>().orthographicSize *= (1f / ZoomSpeed);
            ForgroundCamera.GetComponent<Camera>().orthographicSize *= (1f / ZoomSpeed);
            if (BackgroundCamera.GetComponent<Camera>().orthographicSize < 1f)
            {
                BackgroundCamera.GetComponent<Camera>().orthographicSize = 1f;
                ForgroundCamera.GetComponent<Camera>().orthographicSize = 1f;
            }
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public static Vector2 GetMousePosition()
    {
        if (Instance == null)
        {
            return Vector2.zero;
        }
        else
        {
            return Instance.ForgroundCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
