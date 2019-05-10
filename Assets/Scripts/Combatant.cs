using UnityEngine;

public abstract class Combatant : MonoBehaviour
{
    public int Team { get; protected set; }

    protected void Initialize(int team)
    {
        Team = team;
    }

    public abstract void TakeDamage(Combatant attacker, float damage, DamageType damageType);

    public abstract void PickupExp(float quantity);

    public abstract void PickupGold(float quantity);

    public abstract void PickupFuel(float quantity);

    public abstract void PickupScrap(float quantity);

    public abstract void PickupCrate(int quantity);
}