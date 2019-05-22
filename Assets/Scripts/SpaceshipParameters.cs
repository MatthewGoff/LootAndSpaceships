using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipParameters
{
    public SpaceshipModel Model;
    public string Name;
    public int Team;

    public VehicleType VehicleType;
    public TargetingType TargetingType;
    public float Size;
    public float MassMultiplier;
    public float ThrustForce;
    public float TurnRate;
    public float MaximumSpeed;

    public AIType AIType;
    public string[] AIParameters;

    public float BurnDuration;
    public float MaxShield;
    public float ShieldRegen;
    public float ShieldEnergy;
    public float MaxHealth;
    public float HealthRegen;
    public float MaxEnergy;
    public float EnergyRegen;
    public float MaxFuel;
    public float FuelUsage;
    public float HullSpaceMultiplier;
    public float AttackEnergy;
    public float ThrustEnergy;
    public float LifeSupportEnergy;
    public float LifeSupportDegen;
    public float AttackCooldown;

    public int LootExperience;
    public int LootCredits;
    public int LootFuel;
    public int LootScrap;
    public int LootItems;
}
