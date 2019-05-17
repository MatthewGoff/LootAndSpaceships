using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerController : MonoBehaviour
{
    public float ParticleSpeed;
    public float EmissionRate;
    public float EmissionArc;

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
                GameObject particle = Instantiate(Prefabs.Instance.FlamethrowerParticle, transform.position, Quaternion.identity);
                float angle = Random.Range(-EmissionArc / 2f, EmissionArc / 2f) + transform.eulerAngles.z;
                Vector2 velocity = ParticleSpeed * (Quaternion.Euler(0, 0, angle) * Vector2.right);
                velocity += Manager.Attacker.GetComponent<Rigidbody2D>().velocity;
                particle.GetComponent<FlamethrowerParticleController>().Initialize(Manager, velocity);

            }
        }
    }

    public void AssignManager(FlamethrowerAttackManager manager)
    {
        Manager = manager;
    }

    public void TurnOn(Vector2 attackVector)
    {
        float attackAngle = Vector2.SignedAngle(Vector2.right, attackVector);
        transform.rotation = Quaternion.Euler(0, 0, attackAngle);
        Active = true;
    }

    public void TurnOff()
    {
        Active = false;
    }
}
