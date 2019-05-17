using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerParticleController : MonoBehaviour
{
    public float ParticleLifetime;
    public AnimationCurve ParticleSize;
    public Gradient ParticleColor;

    private FlamethrowerAttackManager Manager;
    private float Age;
    private Vector2 Velocity;

    public void Initialize(FlamethrowerAttackManager manager, Vector2 velocity)
    {
        Manager = manager;
        Age = 0f;
        Velocity = velocity;

        FixedUpdate();
    }

    private void FixedUpdate()
    {
        Age += Time.fixedDeltaTime;
        if (Age > ParticleLifetime)
        {
            Destroy(gameObject);
        }
        transform.position += (Vector3)Velocity * Time.fixedDeltaTime;
        transform.localScale = ParticleSize.Evaluate(Age / ParticleLifetime) * new Vector2(1, 1);
        GetComponent<SpriteRenderer>().color = ParticleColor.Evaluate(Age / ParticleLifetime);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            Spaceship other = collider.GetComponent<Spaceship>();
            if (other.Team != Manager.Attacker.Team)
            {
                Manager.ResolveCollision(other);
            }
        }
    }
}
