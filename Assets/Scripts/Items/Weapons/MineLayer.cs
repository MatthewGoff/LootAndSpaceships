﻿using UnityEngine;

public class MineLayer : Weapon
{
    private readonly float EnergyCost;
    private readonly Cooldown Cooldown;
    private readonly float AOE;
    private readonly float Damage;

    public MineLayer
    (
        float weight,
        float volume,
        float energyCost,
        float cooldown,
        float scrapCost,
        float aoe,
        float damage
    ) : base(weight, volume, ItemType.MineLayer, scrapCost, 0)
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
        new MineAttackManager(attacker, position, AOE, Damage);
    }

    public override void Cease()
    {

    }

    public override float MarginalEnergyCost(float fixedDeltaTime)
    {
        return EnergyCost;
    }

    public static MineLayer CreateRandomMineLayer(int level)
    {
        return new MineLayer(0, 0, 0, 0, 0, 0, 0);
    }
}