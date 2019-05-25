using UnityEngine;

public class BulletAttackManager : AttackManager
{
    private readonly float Damage;

    public BulletAttackManager(Spaceship attacker, Vector2 position, Vector2 direction, Vector2 initialVelocity, float damage, float projectileSpeed, float range)
    {
        Attacker = attacker;
        Damage = damage;

        GameObject bullet = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.Bullet, position, Quaternion.identity);
        bullet.GetComponent<BulletController>().Initialize(this, projectileSpeed, range);
        bullet.GetComponent<Rigidbody2D>().velocity = projectileSpeed * direction.normalized + initialVelocity;
    }

    public void ResolveCollision(Spaceship other)
    {
        other.TakeDamage(this, Damage, DamageType.Physical);
    }
}
