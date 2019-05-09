using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : Vehicle
{
    public GameObject FireEffect;

    public string Name;

    public bool FireBullet;
    public bool FireRocket;
    public bool FireEMP;

    protected bool ShowFDN;

    protected int RadarIdentifier;
    private float BurnDuration;
    private float BurnEndTime;
    private bool Burning;
    private Combatant BurnPerpetrator;
    public float MaxShield;
    public float CurrentShield;
    private float ShieldRegen;
    public float MaxHealth;
    public float CurrentHealth;
    private float HealthRegen;
    public float MaxEnergy;
    public float CurrentEnergy;
    private float EnergyRegen;
    public float MaxFuel;
    public float CurrentFuel;
    private float FuelUsage;
    private float MaxHullSpace;
    private float CurrentHullSpace;
    private Cooldown AttackCooldown;
    protected float AttackEnergy;
    protected float ThrustEnergy;

    protected void Initialize(int team,
        float thrustForce,
        float turnRate,
        float maximumSpeed,
        float mass,
        float burnDuration,
        float maxShield,
        float shieldRegen,
        float maxHP,
        float hpRegen,
        float maxEnergy,
        float energyRegen,
        float maxFuel,
        float fuelUsage,
        float maxHullSpace,
        string name
        )
    {
        base.Initialize(team, thrustForce, turnRate, maximumSpeed, mass);
        BurnDuration = burnDuration;
        MaxShield = maxShield;
        CurrentShield = MaxShield;
        ShieldRegen = shieldRegen;
        MaxHealth = maxHP;
        CurrentHealth = MaxHealth;
        HealthRegen = hpRegen;
        MaxEnergy = maxEnergy;
        CurrentEnergy = MaxEnergy;
        EnergyRegen = energyRegen;
        MaxFuel = maxFuel;
        CurrentFuel = MaxFuel;
        FuelUsage = fuelUsage;
        MaxHullSpace = maxHullSpace;
        CurrentHullSpace = MaxHullSpace;
        Name = name;

        AttackCooldown = new Cooldown(1f);
        AttackEnergy = 1.25f;
        ThrustEnergy = 1.25f;
        RadarIdentifier = RadarOmniscience.Instance.RegisterNewRadarEntity();
    }

    public override void TakeDamage(Combatant attacker, float damage, DamageType damageType)
    {
        float shieldDamage = Mathf.Min(CurrentShield, damage);
        CurrentShield -= shieldDamage;
        float remainingDamage = damage - shieldDamage;
        CurrentHealth -= remainingDamage;

        if (ShowFDN)
        {
            GameObject fdn = Instantiate(Prefabs.Instance.FDN, transform.position, Quaternion.identity);
            fdn.GetComponent<FDNController>().Display(Mathf.RoundToInt(damage), damage / 100f);
        }

        if (damageType == DamageType.Explosion)
        {
            BurnPerpetrator = attacker;
            BurnEndTime = Time.time + BurnDuration;
            if (!Burning)
            {
                StartCoroutine("Burn");
            }
        }
    }

    private IEnumerator Burn()
    {
        Burning = true;
        FireEffect.SetActive(true);

        while (Time.time < BurnEndTime)
        {
            TakeDamage(BurnPerpetrator, Mathf.RoundToInt(Random.Range(0f, 10f)), DamageType.Fire);
            yield return new WaitForSeconds(0.1f);
        }

        Burning = false;
        FireEffect.SetActive(false);
    }

    private void FixedUpdate()
    {
        float thrustCost = ThrustEnergy * Time.fixedDeltaTime;
        if (ThrustInput && CurrentEnergy >= thrustCost)
        {
            CurrentEnergy -= thrustCost;
        }
        else
        {
            ThrustInput = false;
        }

        UpdateVehicle();
        SubmitRadarProfile();

        if (FireBullet && AttackCooldown.Use() && CurrentEnergy >= AttackEnergy)
        {
            FireBullet = false;
            CurrentEnergy -= AttackEnergy;
            int damage = Mathf.RoundToInt(Random.Range(60, 100));
            new BulletAttackManager(this, Position, Heading, Velocity, damage);
            RB2D.AddForce(-Heading * BulletAttackManager.Recoil, ForceMode2D.Impulse);
        }
        if (FireRocket && AttackCooldown.Use() && CurrentEnergy >= AttackEnergy)
        {
            FireRocket = false;
            CurrentEnergy -= AttackEnergy;
            int damage = Mathf.RoundToInt(Random.Range(10f, 30f));
            new RocketAttackManager(this, GameManager.Instance.PlayerTarget, Position, Heading, Velocity, damage);
            RB2D.AddForce(-Heading * RocketAttackManager.Recoil, ForceMode2D.Impulse);
        }
        if (FireEMP && AttackCooldown.Use() && CurrentEnergy >= AttackEnergy)
        {
            FireEMP = false;
            CurrentEnergy -= AttackEnergy;
            int damage = Mathf.RoundToInt(Random.Range(30f, 60f));
            new EMPAttackManager(this, Position, damage);
        }

        CurrentHealth += HealthRegen * Time.fixedDeltaTime;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        CurrentShield += ShieldRegen * Time.fixedDeltaTime;
        CurrentShield = Mathf.Clamp(CurrentShield, 0, MaxShield);
        CurrentEnergy += EnergyRegen * Time.fixedDeltaTime;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
        CurrentFuel -= FuelUsage * Time.fixedDeltaTime;
        CurrentFuel = Mathf.Clamp(CurrentFuel, 0, MaxFuel);
    }

    private void SubmitRadarProfile()
    {
        RadarProfile profile = new RadarProfile(
            Name,
            Team,
            Position,
            MaxShield,
            CurrentShield,
            MaxHealth,
            CurrentHealth,
            MaxEnergy,
            CurrentEnergy,
            MaxFuel,
            CurrentFuel);
        RadarOmniscience.Instance.SubmitRadarProfile(RadarIdentifier, profile);
    }

    public LinkedList<RadarProfile> GetRadarReading()
    {
        return RadarOmniscience.Instance.PingRadar(RadarIdentifier);
    }
}
