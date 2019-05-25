using UnityEngine;

public class Hangar : Weapon
{
    private readonly float EnergyCost;
    private readonly Cooldown Cooldown;
    private readonly string MinionConfiguration;

    public Hangar
    (
        float weight,
        float volume,
        float energyCost,
        float cooldown,
        string minionConfiguration
    ) : base(weight, volume, WeaponType.Hangar, 10, 0)
    {
        EnergyCost = energyCost;
        Cooldown = new Cooldown(cooldown);
        MinionConfiguration = minionConfiguration;
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
        GameObject prefab = SpaceshipPrefabs.Instance.Prefabs[SpaceshipTable.Instance.GetHullParameters(MinionConfiguration).Model];
        GameObject gameObject = GameObject.Instantiate(prefab, attacker.Position, Quaternion.Euler(0f, 0f, attacker.AttackAngle()));
        Spaceship spaceship = gameObject.GetComponent<Spaceship>();
        int UID = Omniscience.Instance.RegisterNewEntity(spaceship);
        Liscense liscense = new Liscense(UID, attacker.Name + "'s Drone", 1, null, null);
        spaceship.Initialize(liscense, MinionConfiguration);
    }

    public override void Cease()
    {

    }

    public override float MarginalEnergyCost(float fixedDeltaTime)
    {
        return EnergyCost;
    }
}
