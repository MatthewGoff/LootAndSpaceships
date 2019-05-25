using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerController : MonoBehaviour
{
    private readonly float ParticleSpeed = 10;
    private readonly float EmissionRate = 60;
    private readonly float EmissionArc = 10;
    private float Range;

    private FlamethrowerAttackManager Manager;
    private bool Active;

    private void FixedUpdate()
    {
        if (Active)
        {
            float particlesToEmit = EmissionRate * Time.fixedDeltaTime;
            float probability = particlesToEmit - Mathf.Floor(particlesToEmit);
            if (Random.Range(0f, 1f) < probability)
            {
                particlesToEmit = Mathf.Ceil(particlesToEmit);
            }
            else
            {
                particlesToEmit = Mathf.Floor(particlesToEmit);
            }
            for (int i = 0; i < particlesToEmit; i++)
            {
                GameObject particle = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.FlamethrowerParticle, transform.position, Quaternion.identity);
                float angle = Random.Range(-EmissionArc / 2f, EmissionArc / 2f) + transform.eulerAngles.z;
                Vector2 velocity = ParticleSpeed * (Quaternion.Euler(0, 0, angle) * Vector2.right);
                velocity += Manager.Attacker.GetComponent<Rigidbody2D>().velocity;
                particle.GetComponent<FlamethrowerParticleController>().Initialize(Manager, velocity, Range);
            }
        }
    }

    public void Initialize(FlamethrowerAttackManager manager, float range)
    {
        Manager = manager;
        Range = range;
    }

    public void TurnOn()
    {
        Active = true;
    }

    public void TurnOff()
    {
        Active = false;
    }
}
