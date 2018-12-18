using System.Collections;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private readonly float Duration = 20f; // In seconds
    private readonly float TurnRate = 90f; // In degrees per second
    private readonly float MaxSpeed = 10f;
    private readonly float Acceleration = 5f;

    private RocketAttackManager Manager;
    private Rigidbody2D RB2D;
    private Vector2 Heading;

    private void Awake()
    {
        RB2D = GetComponent<Rigidbody2D>();
        StartCoroutine("ExpirationCountdown");
        Heading = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.right;
    }

    private IEnumerator ExpirationCountdown()
    {
        yield return new WaitForSeconds(Duration);
        Explode();
    }

    private void Update()
    {
        if (Manager.Target != null)
        {
            Vector2 targetBearing = Manager.Target.GetPosition() - (Vector2)transform.position;
            float angle = Vector2.SignedAngle(Heading, targetBearing);
            angle = Mathf.Clamp(angle, -TurnRate * Time.fixedDeltaTime, TurnRate * Time.fixedDeltaTime);
            Heading = Quaternion.Euler(0, 0, angle) * Heading;
            RB2D.velocity = Quaternion.Euler(0, 0, angle) * RB2D.velocity;
            transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, Heading));
        }
        RB2D.velocity += Heading.normalized * Acceleration * Time.deltaTime;
        RB2D.velocity = RB2D.velocity.normalized * Mathf.Clamp(RB2D.velocity.magnitude, 0, MaxSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            CombatantManager other = collider.gameObject.GetComponent<CombatantManager>();
            if (other.Team != Manager.Originator.Team)
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

    public void AssignManager(RocketAttackManager manager)
    {
        Manager = manager;
    }
}
