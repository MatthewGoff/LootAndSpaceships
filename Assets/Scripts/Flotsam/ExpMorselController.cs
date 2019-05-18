using UnityEngine;

public class ExpMorselController : FlotsamController
{
    public readonly AnimationCurve ALPHA_CURVE = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(0.5f, 0.25f),
        new Keyframe(1f, 1f)
    );
    private readonly float GLOW_PERIOD = 1f;

    private float GlowTime;

    protected override void Start()
    {
        base.Start();
        GlowTime = Random.Range(0, GLOW_PERIOD);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
            Spaceship spaceship = collider.gameObject.GetComponent<Spaceship>();
            if (spaceship.PlayerControlled)
            {
                spaceship.PickupExp(1);
                Destroy(gameObject);
            }
        }
    }
}
