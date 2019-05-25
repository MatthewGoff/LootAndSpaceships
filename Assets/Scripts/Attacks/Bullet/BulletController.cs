using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float Speed;
    private float Range;
    private float Distance;

    private BulletAttackManager Manager;

    public void Initialize(BulletAttackManager manager, float speed, float range)
    {
        Manager = manager;
        Speed = speed;
        Range = range;
        Distance = 0;
    }

    private void FixedUpdate()
    {
        Distance += Speed * Time.deltaTime;
        if (Distance > Range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            Spaceship other = collider.GetComponent<Spaceship>();
            if (other.Team != Manager.Attacker.Team)
            {
                Destroy(gameObject);
                Manager.ResolveCollision(other);
            }
        }
    }
}
