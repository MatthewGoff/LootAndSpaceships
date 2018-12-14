using UnityEngine;

public class BulletAttackManager : AttackManager
{
    public static readonly float Recoil = 50f;
    private static readonly GameObject BULLET_PREFAB = (GameObject)Resources.Load("Prefabs/Bullet");


    private float Speed = 10f; // In units per second
    private int Damage;

    public BulletAttackManager(CombatantManager originator, Vector2 position, Vector2 direction, Vector2 initialVelocity, int damage)
    {
        Originator = originator;
        Damage = damage;

        GameObject bullet = GameObject.Instantiate(BULLET_PREFAB, position, Quaternion.identity);
        bullet.GetComponent<BulletController>().AssignManager(this);
        bullet.GetComponent<Rigidbody2D>().velocity = Speed * direction.normalized + initialVelocity;
    }

    public void ResolveCollision(CombatantManager other)
    {
        other.RecieveHit(Damage, DamageType.Physical);
    }
}
