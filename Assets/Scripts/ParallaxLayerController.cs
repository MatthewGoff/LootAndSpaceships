using UnityEngine;

public class ParallaxLayerController : MonoBehaviour
{
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
        float cameraArea = MasterCameraController.MAX_CAMERA_HEIGHT * MasterCameraController.MAX_CAMERA_WIDTH;
        int numStars = Mathf.RoundToInt(cameraArea * StarDensity);
        Particles = new ParticleSystem.Particle[numStars];
        for (int i = 0; i < Particles.Length; i++)
        {
            Particles[i].position = new Vector2(Random.Range(-MasterCameraController.MAX_CAMERA_WIDTH / 2, MasterCameraController.MAX_CAMERA_WIDTH / 2), Random.Range(-MasterCameraController.MAX_CAMERA_HEIGHT / 2, MasterCameraController.MAX_CAMERA_HEIGHT / 2));
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
            if (Particles[i].position.x > MasterCameraController.MAX_CAMERA_WIDTH / 2)
            {
                Particles[i].position += new Vector3(-MasterCameraController.MAX_CAMERA_WIDTH, 0, 0);
            }
            else if (Particles[i].position.x < -MasterCameraController.MAX_CAMERA_WIDTH / 2)
            {
                Particles[i].position += new Vector3(MasterCameraController.MAX_CAMERA_WIDTH, 0, 0);
            }
            if (Particles[i].position.y > MasterCameraController.MAX_CAMERA_HEIGHT / 2)
            {
                Particles[i].position += new Vector3(0, -MasterCameraController.MAX_CAMERA_HEIGHT, 0);
            }
            else if (Particles[i].position.y < -MasterCameraController.MAX_CAMERA_HEIGHT / 2)
            {
                Particles[i].position += new Vector3(0, MasterCameraController.MAX_CAMERA_HEIGHT, 0);
            }
        }
        ParticleSystem.SetParticles(Particles, Particles.Length);
	}
}
