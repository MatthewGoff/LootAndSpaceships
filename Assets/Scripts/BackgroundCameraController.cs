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
    public float MaxParallaxCoefficient;
    public int NumStarVariants;

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

    private void Awake()
    {
        Camera = GetComponent<Camera>();
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
        ParallaxLayers[layer, materialVariant] = Instantiate(ParallaxLayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        ParallaxLayers[layer, materialVariant].transform.SetParent(StarsTransform.transform);
        ParallaxLayers[layer, materialVariant].GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material = StarMaterial[materialVariant];
        ParallaxLayerController script = ParallaxLayers[layer, materialVariant].GetComponent<ParallaxLayerController>();
        float starDensity = MaxStarDensity[materialVariant] - (MaxStarDensity[materialVariant] - MinStarDensity[materialVariant]) * (float)layer / (float)NumParallaxLayers;
        starDensity /= (float)NumParallaxLayers;
        float starSize = MinStarSize[materialVariant] + (MaxStarSize[materialVariant] - MinStarSize[materialVariant]) * (float)layer / (float)NumParallaxLayers;
        starSize *= Random.Range(MinStarSizeMultiplier[materialVariant], MaxStarSizeMultiplier[materialVariant]);
        float parallaxCoefficient = MaxParallaxCoefficient * (float)layer / (float)ParallaxLayers.Length;
        script.Initialize(starDensity, starSize, parallaxCoefficient);
    }

    private void OnPostRender()
    {
        DrawGridLines();
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
