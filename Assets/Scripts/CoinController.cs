using UnityEngine;

public class CoinController : MonoBehaviour
{
    public readonly AnimationCurve FLIP_CURVE = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(0.5f, 0.1f),
        new Keyframe(1f, 1f)
    );
    private readonly float INITIAL_VELOCITY = 5f;
    private readonly float FLIP_PERIOD = 1f;
    private readonly float ROTATION_SPEED = 90f; // In degrees per second

    private Vector2 InitialScale;
    private Vector2 Velocity;
    private float FlipTime;

    private void Start()
    {
        InitialScale = transform.localScale;
        Velocity = INITIAL_VELOCITY * Random.insideUnitCircle;
        FlipTime = Random.Range(0, FLIP_PERIOD);
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360)) * transform.rotation;
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)Velocity * Time.fixedDeltaTime;
        Velocity *= Mathf.Pow(0.5f, Time.fixedDeltaTime);

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
            Combatant combatant = collider.gameObject.GetComponent<Combatant>();
            if (combatant.Team == 0)
            {
                combatant.PickupGold(1f);
                Destroy(gameObject);
            }
        }
    }
}
