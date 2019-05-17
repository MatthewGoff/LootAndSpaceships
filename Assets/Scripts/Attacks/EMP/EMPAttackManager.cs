using UnityEngine;

public class EMPAttackManager : AttackManager
{
    private float AOE = 4f;
    private int Damage;

    public EMPAttackManager(Spaceship attacker, Vector2 position, int damage)
    {
        Attacker = attacker;
        Damage = damage;

        GameObject explosion = GameObject.Instantiate(Prefabs.Instance.EMP, position, Quaternion.identity);
        explosion.GetComponent<EMPController>().AssignManager(this);
        explosion.transform.localScale = new Vector2(AOE, AOE);
    }

    public void ResolveCollision(Spaceship other)
    {
        other.TakeDamage(this, Damage, DamageType.Electrical);
    }
}
