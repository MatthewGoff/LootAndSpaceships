using UnityEngine;

public class BackgroundCameraController : MonoBehaviour {

    public AnimationCurve Dropoff1s = new AnimationCurve(
        new Keyframe(0f, 0.2f),
        new Keyframe(5f, 0.1f),
        new Keyframe(30f, 0.1f),
        new Keyframe(35f, 0f)
    );

    public AnimationCurve Dropoff10s = new AnimationCurve(
        new Keyframe(0f, 0.2f),
        new Keyframe(5f, 0.2f),
        new Keyframe(30f, 0.2f),
        new Keyframe(35f, 0.2f)
    );

    public GameObject ParallaxLayerPrefab;
    public int NumParallaxLayers;
    public float MinParallaxCoefficient;
    public float MaxParallaxCoefficient;
    public int NumStarVariants;
    public float BackgroundHueRange;
    public readonly float[,] BackgroundHueFrequencies = new float[,] { { 0.1f, 0.2f, 0.3f, 0.4f }, { 0.03f, 0.005f, 0.001f, 0.0002f} };
    public readonly float[,] BackgroundSaturationFrequencies = new float[,] { { 0.1f, 0.2f, 0.3f, 0.4f }, { 0.03f, 0.005f, 0.001f, 0.0002f } };
    public readonly float[,] BackgroundValueFrequencies = new float[,] { { 0.1f, 0.2f, 0.3f, 0.4f }, { 0.03f, 0.005f, 0.001f, 0.0002f } };
    public float BackgroundMinimumSaturation;
    public float BackgroundMaximumValue;

    public Material[] StarMaterial;
    public float[] MinStarSize;
    public float[] MaxStarSize;
    public float[] MinStarDensity;
    public float[] MaxStarDensity;
    public float[] MinStarSizeMultiplier;
    public float[] MaxStarSizeMultiplier;

    private GameObject[,] ParallaxLayers;
    private GameObject StarsTransform;
    private Camera Camera;
    private bool ShowGridLines = true;

    private void Awake()
    {
        Camera = GetComponent<Camera>();
        //CreateBackgroundImage();
        CreateBackgroundStars();
    }

    private void CreateBackgroundImage()
    {
        Texture2D backgroundTexture = CreateBackgroundTexture();
        Rect backgroundRect = new Rect(0, 0, 1920 * 3, 1080 * 3);
        Sprite backgroundSprite = Sprite.Create(backgroundTexture, backgroundRect, new Vector2(0.5f, 0.5f), 1080 * 3);
        GameObject backgroundObject = new GameObject("BackgroundImage");
        backgroundObject.transform.position = new Vector3(0, 0, -2);
        backgroundObject.transform.localScale = new Vector3(MasterCameraController.MAX_CAMERA_HEIGHT, MasterCameraController.MAX_CAMERA_HEIGHT, 1);
        backgroundObject.transform.SetParent(transform);
        backgroundObject.AddComponent<SpriteRenderer>();
        backgroundObject.GetComponent<SpriteRenderer>().sprite = backgroundSprite;
        backgroundObject.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
        backgroundObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    private Texture2D CreateBackgroundTexture()
    {
        Texture2D backgroundTexture = new Texture2D(1920 * 3, 1080 * 3);
        float baseHue = Random.Range(0f, 1f);
        float hueOffset = Random.Range(0f, 100000f);
        float saturationOffset = Random.Range(0f, 100000f);
        float valueOffset = Random.Range(0f, 100000f);
        for (int x = 0; x < backgroundTexture.width; x++)
        {
            for (int y = 0; y < backgroundTexture.height; y++)
            {
                float noiseSample = Helpwers.PerlinNoise(x, y, BackgroundHueFrequencies, hueOffset);
                float hue = (baseHue + BackgroundHueRange * noiseSample) % 1;
                noiseSample = Helpwers.PerlinNoise(x, y, BackgroundSaturationFrequencies, saturationOffset);
                float saturation = BackgroundMinimumSaturation + (1f-BackgroundMinimumSaturation) * noiseSample;
                noiseSample = Helpwers.PerlinNoise(x, y, BackgroundValueFrequencies, valueOffset);
                float value = BackgroundMaximumValue * noiseSample;
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
    
    private void CreateBackgroundStars()
    {
        StarsTransform = new GameObject("Stars");
        StarsTransform.SetActive(true);
        StarsTransform.transform.SetParent(transform);
        CreateParallaxLayers();
    }

    private void CreateParallaxLayers()
    {
        ParallaxLayers = new GameObject[NumParallaxLayers, 2];
        for (int layer = 0; layer < ParallaxLayers.GetLength(0); layer++)
        {
            for (int materialVariant = 0; materialVariant < NumStarVariants; materialVariant++)
            {
                CreateParallaxLayer(layer, materialVariant);
            }
        }
    }
    private void CreateParallaxLayer(int layer, int materialVariant)
    {
        ParallaxLayers[layer, materialVariant] = Instantiate(ParallaxLayerPrefab, new Vector3(0, 0, -2), Quaternion.identity);
        ParallaxLayers[layer, materialVariant].transform.SetParent(StarsTransform.transform);
        ParallaxLayers[layer, materialVariant].GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material = StarMaterial[materialVariant];
        ParallaxLayerController script = ParallaxLayers[layer, materialVariant].GetComponent<ParallaxLayerController>();
        float starDensity = MaxStarDensity[materialVariant] - (MaxStarDensity[materialVariant] - MinStarDensity[materialVariant]) * (float)layer / (float)NumParallaxLayers;
        starDensity /= (float)NumParallaxLayers;
        float starSize = MinStarSize[materialVariant] + (MaxStarSize[materialVariant] - MinStarSize[materialVariant]) * (float)layer / (float)NumParallaxLayers;
        starSize *= Random.Range(MinStarSizeMultiplier[materialVariant], MaxStarSizeMultiplier[materialVariant]);
        float parallaxCoefficient = MinParallaxCoefficient + (MaxParallaxCoefficient - MinParallaxCoefficient) * (float)layer / (float)ParallaxLayers.Length;
        script.Initialize(starDensity, starSize, parallaxCoefficient);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ShowGridLines = !ShowGridLines;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            StarsTransform.SetActive(!StarsTransform.activeSelf);
        }
    }
    private void OnPostRender()
    {
        if (ShowGridLines)
        {
            DrawGridLines();
        }
    }

    private void DrawGridLines()
    {
        float height = 2f * Camera.orthographicSize;
        float width = height * Camera.aspect;
        float top = transform.position.y + height / 2f;
        float bottom = transform.position.y - height / 2f;
        float left = transform.position.x - width / 2f;
        float right = transform.position.x + width / 2f;

        Material lineMat = new Material(Shader.Find("Sprites/Default"));
        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        Draw1sGrid(top, bottom, left, right);
        Draw10sGrid(top, bottom, left, right);
        GL.End();
    }

    private void Draw1sGrid(float top, float bottom, float left, float right)
    {
        float alpha = Dropoff1s.Evaluate(Camera.orthographicSize);
        GL.Color(new Color(1f, 1f, 1f, alpha));

        for (int i = Mathf.CeilToInt(left); i < right; i++)
        {
            GL.Vertex3(i, bottom, 0f);
            GL.Vertex3(i, top, 0f);
        }

        for (int i = Mathf.CeilToInt(bottom); i < top; i++)
        {
            GL.Vertex3(left, i, 0f);
            GL.Vertex3(right, i, 0f);
        }
    }

    private void Draw10sGrid(float top, float bottom, float left, float right)
    {
        float alpha = Dropoff10s.Evaluate(Camera.orthographicSize);
        GL.Color(new Color(1f, 1f, 1f, alpha));

        for (int i = Mathf.CeilToInt(left / 10) * 10; i < right; i+=10)
        {
            GL.Vertex3(i, bottom, 0f);
            GL.Vertex3(i, top, 0f);
        }

        for (int i = Mathf.CeilToInt(bottom / 10) * 10; i < top; i+=10)
        {
            GL.Vertex3(left, i, 0f);
            GL.Vertex3(right, i, 0f);
        }
    }

}
