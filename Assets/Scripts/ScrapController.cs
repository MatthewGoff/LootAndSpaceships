using UnityEngine;

public class ScrapController : MonoBehaviour
{
    private readonly float INITIAL_VELOCITY = 5f;
    private readonly float ROTATION_SPEED = 90f; // In degrees per second

    private Vector2 Velocity;

    private void Start()
    {
        Velocity = INITIAL_VELOCITY * Random.insideUnitCircle;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360)) * transform.rotation;
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)Velocity * Time.fixedDeltaTime;
        Velocity *= Mathf.Pow(0.5f, Time.fixedDeltaTime);

        UpdateRotation();
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
                combatant.PickupScrap(1f);
                Destroy(gameObject);
            }
        }
    }
}
