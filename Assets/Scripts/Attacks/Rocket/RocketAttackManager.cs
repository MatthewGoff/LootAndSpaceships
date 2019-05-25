using UnityEngine;

public class RocketAttackManager : CollisionTypeAttackManager
{
    public readonly bool HasTarget;
    public readonly int TargetUID;

    private readonly float AOE;
    private readonly float Damage;

    public RocketAttackManager
    (
        Spaceship attacker,
        bool hasTarget,
        int targetUID,
        Vector2 position,
        Vector2 direction,
        Vector2 initialVelocity,
        float range,
        float initialSpeed,
        float maximumSpeed,
        float acceleration,
        float turnRate,
        float aoe,
        float damage
    )
    {
        Attacker = attacker;
        HasTarget = hasTarget;
        TargetUID = targetUID;
        AOE = aoe;
        Damage = damage;

        float angle = Vector2.SignedAngle(Vector2.right, direction);
        GameObject rocket = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.Rocket, position, Quaternion.Euler(0, 0, angle));
        rocket.GetComponent<RocketController>().Initialize(this, range, turnRate, maximumSpeed, acceleration);
        rocket.GetComponent<Rigidbody2D>().velocity = initialSpeed * direction.normalized + initialVelocity;
    }

    public void Explode(Vector2 position)
    {
        GameObject explosion = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.Explosion, position, Quaternion.identity);
        explosion.GetComponent<ExplosionController>().Initialize(this);
        explosion.transform.localScale = new Vector2(AOE, AOE);
    }

    public override void ResolveCollision(Spaceship other)
    {
        other.TakeDamage(this, Damage, DamageType.Explosion);
    }
}
