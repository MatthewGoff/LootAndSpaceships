using UnityEngine;

public abstract class Combatant : RadarTarget
{
    public int Team { get; protected set; }

    protected void Initialize(RadarType radarType, int team)
    {
        Team = team;
        base.Initialize(radarType);
    }

    public abstract void RecieveHit(int damage, DamageType damageType);
}
