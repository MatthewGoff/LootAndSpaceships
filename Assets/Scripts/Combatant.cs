using UnityEngine;

public abstract class Combatant : MonoBehaviour, ITargetable
{
    public int Team { get; protected set; }

    protected void Initialize(int team)
    {
        Team = team;
    }

    public abstract void TakeDamage(Combatant attacker, float damage, DamageType damageType);

    public abstract Vector2 GetPosition();
}
