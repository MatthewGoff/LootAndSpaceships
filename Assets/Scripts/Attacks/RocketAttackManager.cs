using UnityEngine;

public class RocketAttackManager : AttackManager
{
    public static readonly float Recoil = 1f;
    private static readonly GameObject ROCKET_PREFAB = (GameObject)Resources.Load("Prefabs/Rocket");
    private static readonly GameObject EXPLOSION_PREFAB = (GameObject)Resources.Load("Prefabs/Explosion");

    public ITargetable Target;

    private float AOE = 2f;
    private float InitialSpeed = 1f; // In units per second
    private int Damage;

    public RocketAttackManager(CombatantManager originator, ITargetable target, Vector2 position, Vector2 direction, Vector2 initialVelocity, int damage)
    {
        Originator = originator;
        Target = target;
        Damage = damage;

        float angle = Vector2.SignedAngle(Vector2.right, direction);
        GameObject rocket = GameObject.Instantiate(ROCKET_PREFAB, position, Quaternion.Euler(0, 0, angle));
        rocket.GetComponent<RocketController>().AssignManager(this);
        rocket.GetComponent<Rigidbody2D>().velocity = InitialSpeed * direction.normalized + initialVelocity;
    }

    public void Explode(Vector2 position)
    {
        GameObject explosion = GameObject.Instantiate(EXPLOSION_PREFAB, position, Quaternion.identity);
        explosion.GetComponent<ExplosionController>().AssignManager(this);
        explosion.transform.localScale = new Vector2(AOE, AOE);
    }

    public void ResolveCollision(CombatantManager other)
    {
        other.RecieveHit(Damage, DamageType.Explosion);
    }
}
