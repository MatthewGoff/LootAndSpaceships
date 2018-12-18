using UnityEngine;

public abstract class CombatantManager : RadarTarget
{
    public int Team { get; protected set; }

    public abstract void RecieveHit(int damage, DamageType damageType);
}
