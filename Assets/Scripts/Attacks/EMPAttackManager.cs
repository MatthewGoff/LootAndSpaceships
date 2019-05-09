using UnityEngine;

public class EMPAttackManager : AttackManager
{
    private float AOE = 4f;
    private int Damage;

    public EMPAttackManager(Combatant attacker, Vector2 position, int damage)
    {
        Attacker = attacker;
        Damage = damage;

        GameObject explosion = GameObject.Instantiate(Prefabs.Instance.EMP, position, Quaternion.identity);
        explosion.GetComponent<EMPController>().AssignManager(this);
        explosion.transform.localScale = new Vector2(AOE, AOE);
    }

    public void ResolveCollision(Combatant other)
    {
        other.TakeDamage(Attacker, Damage, DamageType.Electrical);
    }
}
