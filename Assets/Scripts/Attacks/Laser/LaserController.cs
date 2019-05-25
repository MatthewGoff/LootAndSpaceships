using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    private readonly float PULSE_PERIOD = 0.5f;

    public Gradient Gradient;

    private float StopWatch;
    private LaserAttackManager Manager;
    private GameObject LaserTip;
    private List<Spaceship> Spaceships;
    private bool Active;

    public void Initialize(LaserAttackManager manager, float range)
    {
        Manager = manager;
        StopWatch = 0f;
        Spaceships = new List<Spaceship>();
        Active = false;

        LaserTip = Instantiate(GeneralPrefabs.Instance.LaserTip, Vector2.zero, Quaternion.identity);
        LaserTip.transform.SetParent(transform);
        LaserTip.transform.localPosition = new Vector2(1, 0);
        LaserTip.transform.localRotation = Quaternion.identity;
        LaserTip.transform.localScale = new Vector2(Manager.BEAM_WIDTH / range, 1);
    }

    private void Update()
    {
        if (Active)
        {
            StopWatch = (StopWatch + Time.deltaTime) % PULSE_PERIOD;
            Color color = Gradient.Evaluate(StopWatch / PULSE_PERIOD);
            GetComponent<SpriteRenderer>().color = color;
            LaserTip.GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void FixedUpdate()
    {
        if (Active)
        {
            // Copy the list in case it is modified during enumeration
            List<Spaceship> spaceships = new List<Spaceship>(Spaceships);
            foreach (Spaceship spaceship in spaceships)
            {
                Manager.ResolveCollision(spaceship);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            Spaceship other = collider.gameObject.GetComponent<Spaceship>();
            if (other.Team != Manager.Attacker.Team)
            {
                if (!Spaceships.Contains(other))
                {
                    Spaceships.Add(other);
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            Spaceship other = collider.gameObject.GetComponent<Spaceship>();
            if (other.Team != Manager.Attacker.Team)
            {
                if (Spaceships.Contains(other))
                {
                    Spaceships.Remove(other);
                }
            }
        }
    }

    public void TurnOn(Vector2 attackVector, bool hasTarget, int targetUID)
    {
        float angle = 0f;
        if (hasTarget)
        {
            Vector2 position = Omniscience.Instance.PingRadar()[targetUID].Position;
            Vector2 targetVector = position - (Vector2)transform.position;
            angle = Vector2.SignedAngle(attackVector, targetVector);
            angle = Mathf.Clamp(angle, -Manager.ANGLE_LENIENCE, Manager.ANGLE_LENIENCE);
        }
        float attackAngle = Vector2.SignedAngle(Vector2.right, attackVector);
        transform.rotation = Quaternion.Euler(0, 0, angle + attackAngle);

        GetComponent<SpriteRenderer>().enabled = true;
        LaserTip.GetComponent<SpriteRenderer>().enabled = true;
        Active = true;
    }

    public void TurnOff()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        LaserTip.GetComponent<SpriteRenderer>().enabled = false;
        Active = false;
    }
}
