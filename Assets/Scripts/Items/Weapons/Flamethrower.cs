using UnityEngine;

public class Flamethrower : Weapon
{
    private readonly float EnergyCost;
    private readonly float Range;
    private readonly float Damage;
    private FlamethrowerAttackManager AttackManager;

    public Flamethrower
    (
        float weight,
        float volume,
        float energyCost,
        float range,
        float damage
    ) : base(weight, volume, ItemType.Flamethrower, 0, 0)
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
            AttackManager = new FlamethrowerAttackManager(attacker, Range, Damage);
        }
        AttackManager.TurnOn();
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

    public static Flamethrower CreateRandomFlamethrower(int level)
    {
        return new Flamethrower(0, 0, 0, 0, 0);
    }
}