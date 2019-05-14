using UnityEngine;

public class ScrapController : FlotsamController
{
    private readonly float ROTATION_SPEED = 90f; // In degrees per second

    protected override void Start()
    {
        base.Start();
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360)) * transform.rotation;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
