using UnityEngine;

public class MineAttackManager : CollisionTypeAttackManager
{
    private readonly float AOE = 2f;
    private readonly int Damage;

    public MineAttackManager(Spaceship attacker, Vector2 position, int damage)
    {
        Attacker = attacker;
        Damage = damage;

        GameObject mine = GameManager.Instance.Instantiate(Prefabs.Instance.Mine, position, Quaternion.identity);
        mine.GetComponent<MineController>().Initialize(this);
    }

    public void Explode(Vector2 position)
    {
        GameObject explosion = GameManager.Instance.Instantiate(Prefabs.Instance.Explosion, position, Quaternion.identity);
        explosion.GetComponent<ExplosionController>().AssignManager(this);
        explosion.transform.localScale = new Vector2(AOE, AOE);
    }

    public override void ResolveCollision(Spaceship other)
    {
        other.TakeDamage(this, Damage, DamageType.Explosion);
    }
}
