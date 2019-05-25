using UnityEngine;

public class EMPAttackManager : AttackManager
{
    private readonly float Damage;

    public EMPAttackManager(Spaceship attacker, Vector2 position, float aoe, float damage)
    {
        Attacker = attacker;
        Damage = damage;

        GameObject explosion = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.EMP, position, Quaternion.identity);
        explosion.GetComponent<EMPController>().Initialize(this);
        explosion.transform.localScale = new Vector2(aoe, aoe);
    }

    public void ResolveCollision(Spaceship other)
    {
        other.TakeDamage(this, Damage, DamageType.Electrical);
    }
}
