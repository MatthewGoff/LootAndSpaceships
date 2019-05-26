using UnityEngine;

public class Cannon : Weapon
{
    private readonly float EnergyCost;
    private readonly Cooldown Cooldown;
    private readonly float ProjectileSpeed;
    private readonly float Range;
    private readonly float Damage;

    public Cannon
    (
        float weight,
        float volume,
        float energyCost,
        float cooldown,
        float scrapCost,
        float recoil,
        float projectileSpeed,
        float range,
        float damage
    ) : base(weight, volume, ItemType.Cannon, scrapCost, recoil)
    {
        EnergyCost = energyCost;
        Cooldown = new Cooldown(cooldown);
        ProjectileSpeed = projectileSpeed;
        Range = range;
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
        new BulletAttackManager(attacker, position, direction, initialVelocity, Damage, ProjectileSpeed, Range);
    }

    public override void Cease()
    {

    }

    public override float MarginalEnergyCost(float fixedDeltaTime)
    {
        return EnergyCost;
    }

    public static Cannon CreateRandomCannon(int level)
    {
        return new Cannon(0, 0, 0, 0, 0, 0, 0, 0, 0);
    }
}
