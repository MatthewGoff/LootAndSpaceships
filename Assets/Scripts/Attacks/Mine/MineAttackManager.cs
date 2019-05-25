using UnityEngine;

public class MineAttackManager : CollisionTypeAttackManager
{
    private readonly float AOE;
    private readonly float Damage;

    public MineAttackManager(Spaceship attacker, Vector2 position, float aoe, float damage)
    {
        Attacker = attacker;
        AOE = aoe;
        Damage = damage;

        GameObject mine = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.Mine, position, Quaternion.identity);
        mine.GetComponent<MineController>().Initialize(this);
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
