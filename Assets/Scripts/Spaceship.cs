using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : Vehicle
{
    public GameObject PortraitCamera;
    public GameObject FireEffect;

    public string Name;

    public bool FireBullet;
    public bool FireRocket;
    public bool FireEMP;
    public bool FireHarpoon;
    public bool FireFlamethrower;
    public bool HasTarget;
    public int TargetUID;

    protected bool ShowFDN;

    protected int UID;
    private float BurnDuration;
    private float BurnEndTime;
    private bool Burning;
    public float MaxShield;
    public float CurrentShield;
    private float ShieldRegen;
    private float ShieldEnergy;
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
    protected float LifeSupportEnergy;
    protected float LifeSupportDegen;

    private HarpoonAttackManager Harpoon;
    private FlamethrowerAttackManager Flamethrower;
    private List<AttackImmunityRecord> Immunities;

    protected void Initialize(int team,
        float thrustForce,
        float turnRate,
        float maximumSpeed,
        float mass,
        float burnDuration,
        float maxShield,
        float shieldRegen,
        float shieldEnergy,
        float maxHP,
        float hpRegen,
        float maxEnergy,
        float energyRegen,
        float maxFuel,
        float fuelUsage,
        float maxHullSpace,
        string name,
        float attackEnergy,
        float thrustEnergy,
        float lifeSupportEnergy,
        float lifeSupportDegen
        )
    {
        base.Initialize(team, thrustForce, turnRate, maximumSpeed, mass);
        BurnDuration = burnDuration;
        MaxShield = maxShield;
        CurrentShield = MaxShield;
        ShieldRegen = shieldRegen;
        ShieldEnergy = shieldEnergy;
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
        AttackEnergy = attackEnergy;
        ThrustEnergy = thrustEnergy;
        LifeSupportEnergy = lifeSupportEnergy;
        LifeSupportDegen = lifeSupportDegen;

        AttackCooldown = new Cooldown(1f);
        Flamethrower = new FlamethrowerAttackManager(this, 10);
        Immunities = new List<AttackImmunityRecord>();
        UID = SpaceshipRegistry.Instance.RegisterSpaceship(this);
        RadarOmniscience.Instance.RegisterNewRadarEntity(UID);
    }

    public override void TakeDamage(AttackManager attackManager, float damage, DamageType damageType)
    {
        if (attackManager != null)
        {
            if (ImmuneToDamage(attackManager))
            {
                return;
            }
            else if (attackManager.ImmunityDuration > 0)
            {
                Immunize(attackManager);
            }
        }

        if (ShowFDN)
        {
            GameObject fdn = Instantiate(Prefabs.Instance.FDN, transform.position, Quaternion.identity);
            fdn.GetComponent<FDNController>().Display(Mathf.RoundToInt(damage), damage / 100f);
        }

        if (damageType == DamageType.Explosion || damageType == DamageType.Fire)
        {
            BurnEndTime = Time.time + BurnDuration;
            if (!Burning)
            {
                StartCoroutine("Burn");
            }
        }

        float shieldDamage = Mathf.Min(CurrentShield, damage);
        CurrentShield -= shieldDamage;
        float remainingDamage = damage - shieldDamage;
        CurrentHealth -= remainingDamage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        if (CurrentHealth == 0)
        {
            Die();
        }
    }

    private IEnumerator Burn()
    {
        Burning = true;
        FireEffect.SetActive(true);

        while (Time.time < BurnEndTime)
        {
            TakeDamage(null, Mathf.RoundToInt(Random.Range(0f, 10f)), DamageType.Burning);
            yield return new WaitForSeconds(0.1f);
        }

        Burning = false;
        FireEffect.SetActive(false);
    }

    private void UpdateImmunities()
    {
        List<AttackImmunityRecord> expiredImmunities = new List<AttackImmunityRecord>();
        foreach (AttackImmunityRecord record in Immunities)
        {
            record.RemainingImmunityDuration -= Time.fixedDeltaTime;
            if (record.RemainingImmunityDuration <= 0f)
            {
                expiredImmunities.Add(record);
            }
        }
        foreach (AttackImmunityRecord record in expiredImmunities)
        {
            Immunities.Remove(record);
        }
    }

    private bool ImmuneToDamage(AttackManager attackManager)
    {
        foreach (AttackImmunityRecord record in Immunities)
        {
            if (record.AttackManager == attackManager)
            {
                return true;
            }
        }
        return false;
    }

    private void Immunize(AttackManager attackManager)
    {
        AttackImmunityRecord record = new AttackImmunityRecord
        {
            AttackManager = attackManager,
            RemainingImmunityDuration = attackManager.ImmunityDuration
        };
        Immunities.Add(record);
    }

    private void FixedUpdate()
    {
        UpdateImmunities();
        float energyCost = ThrustEnergy * Time.fixedDeltaTime;
        if (ThrustInput && CurrentEnergy >= energyCost)
        {
            CurrentEnergy -= energyCost;
        }
        else
        {
            ThrustInput = false;
        }

        UpdateVehicle();
        SubmitRadarProfile();

        if (FireBullet && CurrentEnergy >= AttackEnergy && AttackCooldown.Use())
        {
            CurrentEnergy -= AttackEnergy;
            int damage = Mathf.RoundToInt(Random.Range(60, 100));
            new BulletAttackManager(this, Position, Heading, Velocity, damage);
            RB2D.AddForce(-Heading * BulletAttackManager.Recoil, ForceMode2D.Impulse);
        }
        if (FireRocket && CurrentEnergy >= AttackEnergy && AttackCooldown.Use())
        {
            CurrentEnergy -= AttackEnergy;
            int damage = Mathf.RoundToInt(Random.Range(10f, 30f));
            new RocketAttackManager(this, HasTarget, TargetUID, Position, Heading, Velocity, damage);
            RB2D.AddForce(-Heading * RocketAttackManager.Recoil, ForceMode2D.Impulse);
        }
        if (FireEMP && CurrentEnergy >= AttackEnergy && AttackCooldown.Use())
        {
            CurrentEnergy -= AttackEnergy;
            int damage = Mathf.RoundToInt(Random.Range(30f, 60f));
            new EMPAttackManager(this, Position, damage);
        }
        if (FireHarpoon && CurrentEnergy >= AttackEnergy && !HarpoonDeployed() && AttackCooldown.Use())
        {
            CurrentEnergy -= AttackEnergy;
            int damage = Mathf.RoundToInt(Random.Range(20f, 50f));
            Harpoon = new HarpoonAttackManager(this, Position, Heading, Velocity, damage);
        }
        if (FireFlamethrower && CurrentEnergy >= AttackEnergy * Time.fixedDeltaTime)
        {
            CurrentEnergy -= AttackEnergy * Time.fixedDeltaTime;
            Flamethrower.TurnOn();
        }
        else
        {
            Flamethrower.TurnOff();
        }

        energyCost = LifeSupportEnergy * Time.deltaTime;
        if (CurrentEnergy >= energyCost)
        {
            CurrentEnergy -= energyCost;
            CurrentHealth += HealthRegen * Time.fixedDeltaTime;
        }
        else
        {
            CurrentHealth -= LifeSupportDegen;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            if (CurrentHealth == 0)
            {
                Die();
            }
            CurrentEnergy = 0;
        }
            
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        if (CurrentShield < MaxShield)
        {
            energyCost = ShieldEnergy * Time.fixedDeltaTime;
            if (CurrentEnergy >= energyCost)
            {
                CurrentEnergy -= energyCost;
                CurrentShield += ShieldRegen * Time.fixedDeltaTime;
                CurrentShield = Mathf.Clamp(CurrentShield, 0, MaxShield);
            }
        }

        if (CurrentEnergy < MaxEnergy)
        {
            float fuelCost = FuelUsage * Time.fixedDeltaTime;
            if (CurrentFuel >= fuelCost)
            {
                CurrentFuel -= fuelCost;
                CurrentEnergy += EnergyRegen * Time.fixedDeltaTime;
                CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
            }
            else
            {
                CurrentFuel = 0f;
            }
        }
    }

    protected void ZeroInput()
    {
        FireBullet = false;
        FireRocket = false;
        FireEMP = false;
        FireHarpoon = false;
        FireFlamethrower = false;
    }

    private void SubmitRadarProfile()
    {
        RadarProfile profile = new RadarProfile(
            UID,
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
        RadarOmniscience.Instance.SubmitRadarProfile(UID, profile);
    }

    protected virtual void Die()
    {
        CurrentHealth = 0f;
        CurrentShield = 0f;
        CurrentEnergy = 0f;
        CurrentFuel = 0f;

        RadarOmniscience.Instance.UnregisterRadarEntity(UID);
        SpaceshipRegistry.Instance.UnregisterSpaceship(UID);
        Destroy(gameObject);
    }

    public override void PickupExp(int quantity)
    {

    }

    public override void PickupGold(int quantity)
    {

    }

    public override void PickupFuel(float quantity)
    {
        CurrentFuel += quantity;
        CurrentFuel = Mathf.Clamp(CurrentFuel, 0, MaxFuel);
    }

    public override void PickupScrap(float quantity)
    {

    }

    public override void PickupCrate(int quantity)
    {

    }

    public void SelectTarget(int uid)
    {
        HasTarget = true;
        TargetUID = uid;
    }

    public void DropTarget()
    {
        HasTarget = false;
    }

    public Dictionary<int, RadarProfile> GetRadarReading()
    {
        return RadarOmniscience.Instance.PingRadar(UID);
    }

    private bool HarpoonDeployed()
    {
        if (Harpoon == null)
        {
            return false;
        }
        else
        {
            return (Harpoon.State != HarpoonState.Expired);
        }
    }
}
