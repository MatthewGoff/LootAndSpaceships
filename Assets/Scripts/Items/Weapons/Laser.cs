using UnityEngine;

public class Laser : Weapon
{
    private readonly float EnergyCost;
    private readonly float Range;
    private readonly float Damage;
    private LaserAttackManager AttackManager;

    public Laser(float weight, float volume, float energyCost, float range, float damage) : base(weight, volume, WeaponType.Laser, 0, 0)
    {
        EnergyCost = energyCost;
        Range = range;
        Damage = damage;
    }

    public override bool CooldownAvailable()
    {
        return true;
    }

    public override void UseCooldown()
    {

    }

    public override void ExecuteAttack(Spaceship attacker, bool hasTarget, int targetUID, Vector2 position, Vector2 direction, Vector2 initialVelocity)
    {
        if (AttackManager == null)
        {
            AttackManager = new LaserAttackManager(attacker, Range, Damage);
        }
        AttackManager.TurnOn(direction, hasTarget, targetUID);
    }

    public override void Cease()
    {
        if (AttackManager != null)
        {
            AttackManager.TurnOff();
        }

    }

    public override float MarginalEnergyCost(float fixedDeltaTime)
    {
        return EnergyCost;
    }
}