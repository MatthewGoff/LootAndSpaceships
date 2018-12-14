using UnityEngine;

public abstract class CombatantManager : MonoBehaviour, ITargetable
{
    public int Team;

    public abstract Vector2 GetPosition();
    public abstract void RecieveHit(int damage, DamageType damageType);
}
