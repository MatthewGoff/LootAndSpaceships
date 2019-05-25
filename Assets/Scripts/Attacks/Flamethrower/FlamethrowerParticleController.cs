using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerParticleController : MonoBehaviour
{
    public AnimationCurve ParticleSize;
    public Gradient ParticleColor;

    private float Range;
    private FlamethrowerAttackManager Manager;
    private float Distance;
    private Vector2 Velocity;

    public void Initialize(FlamethrowerAttackManager manager, Vector2 velocity, float range)
    {
        Manager = manager;
        Range = range;
        Velocity = velocity;
        Distance = 0;

        FixedUpdate();
    }

    private void FixedUpdate()
    {
        Distance += Velocity.magnitude * Time.fixedDeltaTime;
        if (Distance > Range)
        {
            Destroy(gameObject);
        }
        transform.position += (Vector3)Velocity * Time.fixedDeltaTime;
        transform.localScale = ParticleSize.Evaluate(Distance / Range) * new Vector2(1, 1);
        GetComponent<SpriteRenderer>().color = ParticleColor.Evaluate(Distance / Range);
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
