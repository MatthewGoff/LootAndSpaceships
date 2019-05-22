using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipParameters
{
    public string Name;
    public int Team;

    public VehicleType VehicleType;
    public TargetingType TargetingType;
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
    public float AttackEnergy;
    public float ThrustEnergy;
    public float LifeSupportEnergy;
    public float LifeSupportDegen;
}
