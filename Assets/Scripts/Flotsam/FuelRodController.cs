using UnityEngine;

public class FuelRodController : FlotsamController
{
    public readonly AnimationCurve ALPHA_CURVE = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(0.5f, 0.25f),
        new Keyframe(1f, 1f)
    );
    private readonly float GLOW_PERIOD = 1f;
    private readonly float ROTATION_SPEED = 90f; // In degrees per second

    public GameObject GlowEffect;

    private float GlowTime;

    protected override void Start()
    {
        base.Start();
        GlowTime = Random.Range(0, GLOW_PERIOD);
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360)) * transform.rotation;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateAlpha();
        UpdateRotation();
    }

    private void UpdateAlpha()
    {
        GlowTime += Time.fixedDeltaTime;
        GlowTime %= GLOW_PERIOD;
        float alpha = ALPHA_CURVE.Evaluate(GlowTime / GLOW_PERIOD);
        Color color = GlowEffect.GetComponent<SpriteRenderer>().color;
        color.a = alpha;
        GlowEffect.GetComponent<SpriteRenderer>().color = color;
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
                combatant.PickupFuel(50f);
                Destroy(gameObject);
            }
        }
    }
}
