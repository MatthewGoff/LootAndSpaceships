using UnityEngine;

public class MasterCameraController : MonoBehaviour
{
    public static readonly float DEFAULT_CAMERA_HEIGHT = 20f;
    public static readonly float MIN_CAMERA_HEIGHT = 2f;
    public static readonly float MAX_CAMERA_HEIGHT = 60f;
    public static readonly float MAX_CAMERA_WIDTH = 120f;

    public float BACKGROUND_HUE_RANGE;
    public readonly float[,] BACKGROUND_HUE_FREQUENCIES = new float[,] { { 0.1f, 0.2f, 0.3f, 0.4f }, { 0.03f, 0.005f, 0.001f, 0.0002f } };
    public readonly float[,] BACKGROUND_SATURATION_FREQUENCIES = new float[,] { { 0.1f, 0.2f, 0.3f, 0.4f }, { 0.03f, 0.005f, 0.001f, 0.0002f } };
    public readonly float[,] BACKGROUND_VALUE_FREQUENCIES = new float[,] { { 0.1f, 0.2f, 0.3f, 0.4f }, { 0.03f, 0.005f, 0.001f, 0.0002f } };
    public float BACKGROUND_MINIMUM_SATURATION;
    public float BACKGROUND_MAXIMUM_VALUE;

    public static MasterCameraController Instance;

    public GameObject BackgroundImage;
    public GameObject BackgroundCamera;
    public GameObject ForgroundCamera;
    public ITargetable Subject;

    private readonly float ZoomSpeed = 1.2f;
    private float CameraHeight;

    private void Awake()
    {
        Instance = this;
        CameraHeight = DEFAULT_CAMERA_HEIGHT;
        CreateBackgroundImage();
    }

    private void CreateBackgroundImage()
    {
        Texture2D backgroundTexture = CreateBackgroundTexture();
        Rect backgroundRect = new Rect(0, 0, 1920, 1080);
        Sprite backgroundSprite = Sprite.Create(backgroundTexture, backgroundRect, new Vector2(0.5f, 0.5f), 1080);
        BackgroundImage.GetComponent<SpriteRenderer>().sprite = backgroundSprite;
    }

    private Texture2D CreateBackgroundTexture()
    {
        Texture2D backgroundTexture = new Texture2D(1920, 1080);
        float baseHue = Random.Range(0f, 1f);
        float hueOffset = Random.Range(0f, 100000f);
        float saturationOffset = Random.Range(0f, 100000f);
        float valueOffset = Random.Range(0f, 100000f);
        for (int x = 0; x < backgroundTexture.width; x++)
        {
            for (int y = 0; y < backgroundTexture.height; y++)
            {
                float noiseSample = Helpers.PerlinNoise(x, y, BACKGROUND_HUE_FREQUENCIES, hueOffset);
                float hue = (baseHue + BACKGROUND_HUE_RANGE * noiseSample) % 1;
                noiseSample = Helpers.PerlinNoise(x, y, BACKGROUND_SATURATION_FREQUENCIES, saturationOffset);
                float saturation = BACKGROUND_MINIMUM_SATURATION + (1f - BACKGROUND_MINIMUM_SATURATION) * noiseSample;
                noiseSample = Helpers.PerlinNoise(x, y, BACKGROUND_VALUE_FREQUENCIES, valueOffset);
                float value = BACKGROUND_MAXIMUM_VALUE * noiseSample;
                if (Random.Range(0f, 1f) < 0.005f)
                {
                    saturation = Mathf.Min(0f, saturation - 0.5f);
                    value = Mathf.Max(value + 0.5f, 1f);
                }
                backgroundTexture.SetPixel(x, y, Color.HSVToRGB(hue, saturation, value));
            }
        }
        backgroundTexture.Apply();
        return backgroundTexture;
    }

    private void Update ()
    {
        BackgroundImage.transform.position = new Vector3(Subject.GetPosition().x, Subject.GetPosition().y, BackgroundImage.transform.position.z);
        BackgroundCamera.transform.position = new Vector3(Subject.GetPosition().x, Subject.GetPosition().y, BackgroundCamera.transform.position.z);
        ForgroundCamera.transform.position = new Vector3(Subject.GetPosition().x, Subject.GetPosition().y, ForgroundCamera.transform.position.z);

        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            CameraHeight *= (1f / ZoomSpeed);
            if (CameraHeight < MIN_CAMERA_HEIGHT)
            {
                CameraHeight = MIN_CAMERA_HEIGHT;
            }
        }
        else if (d < 0f)
        {
            CameraHeight *= ZoomSpeed;
            if (CameraHeight > MAX_CAMERA_HEIGHT)
            {
                CameraHeight = MAX_CAMERA_HEIGHT;
            }
        }

        BackgroundImage.transform.localScale = new Vector2(CameraHeight, CameraHeight);
        BackgroundCamera.GetComponent<Camera>().orthographicSize = CameraHeight / 2;
        ForgroundCamera.GetComponent<Camera>().orthographicSize = CameraHeight / 2;

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
