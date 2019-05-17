using System.Collections.Generic;
using UnityEngine;

public class CoinController : FlotsamController
{
    public readonly AnimationCurve FLIP_CURVE = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(0.5f, 0.1f),
        new Keyframe(1f, 1f)
    );
    private readonly float FLIP_PERIOD = 1f;
    private readonly float ROTATION_SPEED = 90f; // In degrees per second

    private Vector2 InitialScale;
    private float FlipTime;

    protected override void Start()
    {
        base.Start();
        InitialScale = transform.localScale;
        FlipTime = Random.Range(0, FLIP_PERIOD);
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360)) * transform.rotation;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateFlip();
        UpdateRotation();
    }

    private void UpdateFlip()
    {
        FlipTime += Time.fixedDeltaTime;
        FlipTime %= FLIP_PERIOD;
        Vector2 scale = transform.localScale;
        scale.y = InitialScale.y * FLIP_CURVE.Evaluate(FlipTime / FLIP_PERIOD);
        transform.localScale = scale;
    }

    private void UpdateRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, ROTATION_SPEED * Time.fixedDeltaTime) * transform.rotation;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            Spaceship combatant = collider.gameObject.GetComponent<Spaceship>();
            if (combatant.Team == 0)
            {
                combatant.PickupGold(1);
                Destroy(gameObject);
            }
        }
    }
}
