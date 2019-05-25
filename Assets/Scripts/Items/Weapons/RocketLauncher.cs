using UnityEngine;

public class RocketLauncher : Weapon
{
    private readonly float EnergyCost;
    private readonly Cooldown Cooldown;
    private readonly float Range;
    private readonly float InitialSpeed;
    private readonly float MaximumSpeed;
    private readonly float Acceleration;
    private readonly float TurnRate;
    private readonly float AOE;
    private readonly float Damage;

    public RocketLauncher
    (
        float weight,
        float volume,
        float energyCost,
        float cooldown,
        float scrapCost,
        float recoil,
        float range,
        float initialSpeed,
        float maximumSpeed,
        float acceleration,
        float turnRate,
        float aoe,
        float damage
    ) : base(weight, volume, WeaponType.RocketLauncher, scrapCost, recoil)
    {
        EnergyCost = energyCost;
        Cooldown = new Cooldown(cooldown);
        Range = range;
        InitialSpeed = initialSpeed;
        MaximumSpeed = maximumSpeed;
        Acceleration = acceleration;
        TurnRate = turnRate;
        AOE = aoe;
        Damage = damage;
    }

    public override bool CooldownAvailable()
    {
        return Cooldown.Available;
    }

    public override void UseCooldown()
    {
        Cooldown.Use();
    }

    public override void ExecuteAttack(Spaceship attacker, bool hasTarget, int targetUID, Vector2 position, Vector2 direction, Vector2 initialVelocity)
    {
        new RocketAttackManager
        (
            attacker,
            hasTarget,
            targetUID,
            position,
            direction,
            initialVelocity,
            Range,
            InitialSpeed,
            MaximumSpeed,
            Acceleration,
            TurnRate,
            AOE,
            Damage
        );
    }

    public override void Cease()
    {

    }

    public override float MarginalEnergyCost(float fixedDeltaTime)
    {
        return EnergyCost;
    }
}
