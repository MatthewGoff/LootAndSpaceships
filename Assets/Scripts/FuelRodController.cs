using UnityEngine;

public class FuelRodController : MonoBehaviour
{
    public readonly AnimationCurve ALPHA_CURVE = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(0.5f, 0.25f),
        new Keyframe(1f, 1f)
    );
    private readonly float INITIAL_VELOCITY = 5f;
    private readonly float GLOW_PERIOD = 1f;
    private readonly float ROTATION_SPEED = 90f; // In degrees per second

    public GameObject GlowEffect;

    private Vector2 Velocity;
    private float GlowTime;

    private void Start()
    {
        Velocity = INITIAL_VELOCITY * Random.insideUnitCircle;
        GlowTime = Random.Range(0, GLOW_PERIOD);
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360)) * transform.rotation;
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)Velocity * Time.fixedDeltaTime;
        Velocity *= Mathf.Pow(0.5f, Time.fixedDeltaTime);

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
            Combatant combatant = collider.gameObject.GetComponent<Combatant>();
            if (combatant.Team == 0)
            {
                combatant.PickupFuel(50f);
                Destroy(gameObject);
            }
        }
    }
}
