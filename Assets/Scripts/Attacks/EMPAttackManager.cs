using UnityEngine;

public class EMPAttackManager : AttackManager
{
    private static readonly GameObject EMP_Prefab = (GameObject)Resources.Load("Prefabs/EMP");

    private float AOE = 4f;
    private int Damage;

    public EMPAttackManager(CombatantManager originator, Vector2 position, int damage)
    {
        Originator = originator;
        Damage = damage;

        GameObject explosion = GameObject.Instantiate(EMP_Prefab, position, Quaternion.identity);
        explosion.GetComponent<EMPController>().AssignManager(this);
        explosion.transform.localScale = new Vector2(AOE, AOE);
    }

    public void ResolveCollision(CombatantManager other)
    {
        other.RecieveHit(Damage, DamageType.Electrical);
    }
}
