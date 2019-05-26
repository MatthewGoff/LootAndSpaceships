using UnityEngine;

public abstract class Weapon : Item
{
    public float ScrapCost;
    public float Recoil;

    protected Weapon(float weight, float volume, ItemType itemType, float scrapCost, float recoil) : base(weight, volume, ItemClass.Weapon, itemType)
    {
        ScrapCost = scrapCost;
        Recoil = recoil;
    }

    public abstract float MarginalEnergyCost(float fixedDeltaTime);
    public abstract bool CooldownAvailable();
    public abstract void UseCooldown();
    public abstract void ExecuteAttack(Spaceship attacker, bool hasTarget, int targetUID, Vector2 position, Vector2 direction, Vector2 initialVelocity);
    public abstract void Cease();

    public static Weapon CreateWeapon(WeaponParameters parameters)
    {
        if (parameters.WeaponType == ItemType.Cannon)
        {
            return new Cannon
            (
                weight: parameters.Weight,
                volume: parameters.Volume,
                energyCost: Helpers.ParseFloat(parameters.Param1),
                cooldown: Helpers.ParseFloat(parameters.Param2),
                scrapCost: Helpers.ParseFloat(parameters.Param3),
                recoil: Helpers.ParseFloat(parameters.Param4),
                projectileSpeed: Helpers.ParseFloat(parameters.Param5),
                range: Helpers.ParseFloat(parameters.Param6),
                damage: Helpers.ParseFloat(parameters.Param7)
            );
        }
        else if (parameters.WeaponType == ItemType.EMPGenerator)
        {
            return new EMPGenerator
            (
                weight: parameters.Weight,
                volume: parameters.Volume,
                energyCost: Helpers.ParseFloat(parameters.Param1),
                cooldown: Helpers.ParseFloat(parameters.Param2),
                aoe: Helpers.ParseFloat(parameters.Param4),
                damage: Helpers.ParseFloat(parameters.Param5)
            );
        }
        else if (parameters.WeaponType == ItemType.Flamethrower)
        {
            return new Flamethrower
            (
                weight: parameters.Weight,
                volume: parameters.Volume,
                energyCost: Helpers.ParseFloat(parameters.Param1),
                range: Helpers.ParseFloat(parameters.Param2),
                damage: Helpers.ParseFloat(parameters.Param3)
            );
        }
        else if (parameters.WeaponType == ItemType.Hangar)
        {
            return new Hangar
            (
                weight: parameters.Weight,
                volume: parameters.Volume,
                energyCost: Helpers.ParseFloat(parameters.Param1),
                cooldown: Helpers.ParseFloat(parameters.Param2),
                minionConfiguration: parameters.Param3
            );
        }
        else if (parameters.WeaponType == ItemType.Harpoon)
        {
            return new Harpoon
            (
                weight: parameters.Weight,
                volume: parameters.Volume,
                energyCost: Helpers.ParseFloat(parameters.Param1),
                cooldown: Helpers.ParseFloat(parameters.Param2),
                recoil: Helpers.ParseFloat(parameters.Param3),
                damage: Helpers.ParseFloat(parameters.Param4),
                speed: Helpers.ParseFloat(parameters.Param5),
                range: Helpers.ParseFloat(parameters.Param6),
                duration: Helpers.ParseFloat(parameters.Param7)
            );
        }
        else if (parameters.WeaponType == ItemType.Laser)
        {
            return new Laser
            (
                weight: parameters.Weight,
                volume: parameters.Volume,
                energyCost: Helpers.ParseFloat(parameters.Param1),
                range: Helpers.ParseFloat(parameters.Param2),
                damage: Helpers.ParseFloat(parameters.Param3)
            );
        }
        else if (parameters.WeaponType == ItemType.MineLayer)
        {
            return new MineLayer
            (
                weight: parameters.Weight,
                volume: parameters.Volume,
                energyCost: Helpers.ParseFloat(parameters.Param1),
                cooldown: Helpers.ParseFloat(parameters.Param2),
                scrapCost: Helpers.ParseFloat(parameters.Param3),
                aoe: Helpers.ParseFloat(parameters.Param4),
                damage: Helpers.ParseFloat(parameters.Param5)
            );
        }
        else if (parameters.WeaponType == ItemType.RocketLauncher)
        {
            return new RocketLauncher
            (
                weight: parameters.Weight,
                volume: parameters.Volume,
                energyCost: Helpers.ParseFloat(parameters.Param1),
                cooldown: Helpers.ParseFloat(parameters.Param2),
                scrapCost: Helpers.ParseFloat(parameters.Param3),
                recoil: Helpers.ParseFloat(parameters.Param4),
                range: Helpers.ParseFloat(parameters.Param5),
                initialSpeed: Helpers.ParseFloat(parameters.Param6),
                maximumSpeed: Helpers.ParseFloat(parameters.Param7),
                acceleration: Helpers.ParseFloat(parameters.Param8),
                turnRate: Helpers.ParseFloat(parameters.Param9),
                aoe: Helpers.ParseFloat(parameters.Param10),
                damage: Helpers.ParseFloat(parameters.Param11)
            );
        }
        else
        {
            return null;
        }
    }
}