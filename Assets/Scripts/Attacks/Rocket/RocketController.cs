using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private float Range;
    private float TurnRate;
    private float MaximumSpeed;
    private float Acceleration;
    private float Distance;

    private RocketAttackManager Manager;
    private Rigidbody2D RB2D;
    private Vector2 Heading;

    public void Initialize(RocketAttackManager manager, float range, float turnRate, float maximumSpeed, float acceleration)
    {
        Manager = manager;
        Range = range;
        TurnRate = turnRate;
        MaximumSpeed = maximumSpeed;
        Acceleration = acceleration;
        Distance = 0;
        RB2D = GetComponent<Rigidbody2D>();
        Heading = Quaternion.Euler(0, 0, RB2D.rotation) * Vector2.right;
    }

    private void FixedUpdate()
    {
        Distance += RB2D.velocity.magnitude * Time.fixedDeltaTime;
        if (Distance > Range)
        {
            Destroy(gameObject);
        }
        Dictionary<int, RadarProfile> radarProfiles = Omniscience.Instance.PingRadar();
        if (Manager.HasTarget && radarProfiles.ContainsKey(Manager.TargetUID))
        {
            Vector2 targetPosition = radarProfiles[Manager.TargetUID].Position;
            Vector2 targetBearing = targetPosition - (Vector2)transform.position;
            float angle = Vector2.SignedAngle(Heading, targetBearing);
            angle = Mathf.Clamp(angle, -TurnRate * Time.fixedDeltaTime, TurnRate * Time.fixedDeltaTime);
            Heading = Quaternion.Euler(0, 0, angle) * Heading;
            RB2D.velocity = Quaternion.Euler(0, 0, angle) * RB2D.velocity;
            transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, Heading));
        }
        RB2D.velocity += Heading.normalized * Acceleration * Time.fixedDeltaTime;
        RB2D.velocity = RB2D.velocity.normalized * Mathf.Clamp(RB2D.velocity.magnitude, 0, MaximumSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            Spaceship other = collider.gameObject.GetComponent<Spaceship>();
            if (other.Team != Manager.Attacker.Team)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        Manager.Explode((Vector2)transform.position + RB2D.velocity.normalized * 0.5f);
        Destroy(gameObject);
    }
}
