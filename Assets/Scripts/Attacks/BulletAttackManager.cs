using UnityEngine;

public class BulletAttackManager : AttackManager
{
    public static readonly float Recoil = 1f;

    private float Speed = 10f; // In units per second
    private int Damage;

    public BulletAttackManager(Combatant attacker, Vector2 position, Vector2 direction, Vector2 initialVelocity, int damage)
    {
        Attacker = attacker;
        Damage = damage;

        GameObject bullet = GameObject.Instantiate(Prefabs.Bullet, position, Quaternion.identity);
        bullet.GetComponent<BulletController>().AssignManager(this);
        bullet.GetComponent<Rigidbody2D>().velocity = Speed * direction.normalized + initialVelocity;
    }

    public void ResolveCollision(Combatant other)
    {
        other.TakeDamage(Attacker, Damage, DamageType.Physical);
    }
}
