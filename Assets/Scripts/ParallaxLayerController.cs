using UnityEngine;

public class ParallaxLayerController : MonoBehaviour
{
    private static readonly float CAMERA_HEIGHT = 60f;
    private static readonly float CAMERA_WIDTH = 120f;

    private ParticleSystem ParticleSystem;
    private ParticleSystem.Particle[] Particles;
    private float StarDensity = 0.1f; // Stars per unit square
    private float StarSize = 0.1f;
    private float ParallaxCoefficient = 0.1f;

    private Vector3 PositionLastUpdate;
    
    public void Initialize(float starDensity, float starSize, float parallaxCoefficient)
    {
        StarDensity = starDensity;
        StarSize = starSize;
        ParallaxCoefficient = parallaxCoefficient;

        ParticleSystem = GetComponent<ParticleSystem>();
        PositionLastUpdate = ParticleSystem.transform.position;
        CreateParticles();
    }

    private void CreateParticles()
    {
        float cameraArea = CAMERA_HEIGHT * CAMERA_WIDTH;
        int numStars = Mathf.RoundToInt(cameraArea * StarDensity);
        Particles = new ParticleSystem.Particle[numStars];
        for (int i = 0; i < Particles.Length; i++)
        {
            Particles[i].position = new Vector2(Random.Range(-CAMERA_WIDTH / 2, CAMERA_WIDTH / 2), Random.Range(-CAMERA_HEIGHT / 2, CAMERA_HEIGHT / 2));
            Particles[i].startColor = new Color(1, 1, 1, 1);
            Particles[i].startSize = StarSize;
        }
    }
    
    public void Update ()
    {
        Vector3 positionChange = PositionLastUpdate - ParticleSystem.transform.position;
        PositionLastUpdate = ParticleSystem.transform.position;

        for (int i = 0; i < Particles.Length; i++)
        {
            Particles[i].position += ParallaxCoefficient * positionChange;
            if (Particles[i].position.x > CAMERA_WIDTH / 2)
            {
                Particles[i].position += new Vector3(-CAMERA_WIDTH, 0, 0);
            }
            else if (Particles[i].position.x < -CAMERA_WIDTH / 2)
            {
                Particles[i].position += new Vector3(CAMERA_WIDTH, 0, 0);
            }
            if (Particles[i].position.y > CAMERA_HEIGHT / 2)
            {
                Particles[i].position += new Vector3(0, -CAMERA_HEIGHT, 0);
            }
            else if (Particles[i].position.y < -CAMERA_HEIGHT / 2)
            {
                Particles[i].position += new Vector3(0, CAMERA_HEIGHT, 0);
            }
        }
        ParticleSystem.SetParticles(Particles, Particles.Length);
	}
}
