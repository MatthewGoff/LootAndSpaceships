using UnityEngine;

public class EMPGenerator : Weapon
{
    private readonly float EnergyCost;
    private readonly Cooldown Cooldown;
    private readonly float AOE;
    private readonly float Damage;

    public EMPGenerator
    (
        float weight,
        float volume,
        float energyCost,
        float cooldown,
        float aoe,
        float damage
    ) : base(weight, volume, WeaponType.EMPGenerator, 0, 0)
    {
        EnergyCost = energyCost;
        Cooldown = new Cooldown(cooldown);
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
        new EMPAttackManager(attacker, position, AOE, Damage);
    }

    public override void Cease()
    {

    }

    public override float MarginalEnergyCost(float fixedDeltaTime)
    {
        return EnergyCost;
    }
}