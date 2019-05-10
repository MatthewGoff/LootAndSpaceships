﻿using UnityEngine;

public class ExpMorselController : MonoBehaviour
{
    public readonly AnimationCurve ALPHA_CURVE = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(0.5f, 0.25f),
        new Keyframe(1f, 1f)
    );
    private readonly float INITIAL_VELOCITY = 5f;
    private readonly float GLOW_PERIOD = 1f;

    private Vector2 Velocity;
    private float GlowTime;

    private void Start()
    {
        Velocity = INITIAL_VELOCITY * Random.insideUnitCircle;
        GlowTime = Random.Range(0, GLOW_PERIOD);
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)Velocity * Time.fixedDeltaTime;
        Velocity *= Mathf.Pow(0.5f, Time.fixedDeltaTime);

        UpdateAlpha();
    }

    private void UpdateAlpha()
    {
        GlowTime += Time.fixedDeltaTime;
        GlowTime %= GLOW_PERIOD;
        float alpha = ALPHA_CURVE.Evaluate(GlowTime / GLOW_PERIOD);
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = alpha;
        GetComponent<SpriteRenderer>().color = color;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            Combatant combatant = collider.gameObject.GetComponent<Combatant>();
            if (combatant.Team == 0)
            {
                combatant.PickupExp(1f);
                Destroy(gameObject);
            }
        }
    }
}
