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

    public void Initialize(LaserAttackManager manager)
    {
        Manager = manager;
        StopWatch = 0f;

        LaserTip = Instantiate(Prefabs.Instance.LaserTip, Vector2.zero, Quaternion.identity);
        LaserTip.transform.SetParent(transform);
        LaserTip.transform.localPosition = new Vector2(1, 0);
        LaserTip.transform.localRotation = Quaternion.identity;
        LaserTip.transform.localScale = new Vector2(Manager.BEAM_WIDTH / Manager.BEAM_LENGTH, 1);
    }

    private void Update()
    {
        StopWatch = (StopWatch + Time.deltaTime) % PULSE_PERIOD;
        Color color = Gradient.Evaluate(StopWatch / PULSE_PERIOD);
        GetComponent<SpriteRenderer>().color = color;
        LaserTip.GetComponent<SpriteRenderer>().color = color;
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            Spaceship other = collider.gameObject.GetComponent<Spaceship>();
            if (other.Team != Manager.Attacker.Team)
            {
                Manager.ResolveCollision(other);
            }
        }
    }
}
