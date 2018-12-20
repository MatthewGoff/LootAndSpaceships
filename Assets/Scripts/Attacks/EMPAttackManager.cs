using UnityEngine;

public class EMPAttackManager : AttackManager
{
    private float AOE = 4f;
    private int Damage;

    public EMPAttackManager(Combatant originator, Vector2 position, int damage)
    {
        Originator = originator;
        Damage = damage;

        GameObject explosion = GameObject.Instantiate(Prefabs.EMP, position, Quaternion.identity);
        explosion.GetComponent<EMPController>().AssignManager(this);
        explosion.transform.localScale = new Vector2(AOE, AOE);
    }

    public void ResolveCollision(Combatant other)
    {
        other.RecieveHit(Damage, DamageType.Electrical);
    }
}
