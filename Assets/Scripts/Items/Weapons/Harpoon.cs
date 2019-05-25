using UnityEngine;

public class Harpoon : Weapon
{
    private readonly float EnergyCost;
    private readonly Cooldown Cooldown;
    private readonly float Damage;
    private readonly float Speed;
    private readonly float Range;
    private readonly float Duration;
    private HarpoonAttackManager AttackManager;
    private bool Deployed
    {
        get
        {
            return (AttackManager == null || AttackManager.State == HarpoonState.Expired);
        }
    }

    public Harpoon
    (
        float weight,
        float volume,
        float energyCost,
        float cooldown,
        float recoil,
        float damage,
        float speed,
        float range,
        float duration
    ) : base(weight, volume, WeaponType.Harpoon, 0, recoil)
    {
        EnergyCost = energyCost;
        Cooldown = new Cooldown(cooldown);
        Damage = damage;
        Speed = speed;
        Range = range;
        Duration = duration;
    }

    public override bool CooldownAvailable()
    {
        return Cooldown.Available && !Deployed;
    }

    public override void UseCooldown()
    {
        Cooldown.Use();
    }

    public override void ExecuteAttack(Spaceship attacker, bool hasTarget, int targetUID, Vector2 position, Vector2 direction, Vector2 initialVelocity)
    {
        AttackManager = new HarpoonAttackManager(attacker, position, direction, initialVelocity, Damage, Speed, Range, Duration);
    }

    public override void Cease()
    {
        
    }

    public override float MarginalEnergyCost(float fixedDeltaTime)
    {
        return EnergyCost;
    }
}