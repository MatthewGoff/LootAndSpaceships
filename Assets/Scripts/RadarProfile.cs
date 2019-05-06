﻿using UnityEngine;

public class RadarProfile
{
    public int Team;
    public Vector2 Position;
    public float MaxShield;
    public float CurrentShield;
    public float MaxHP;
    public float CurrentHP;
    public float MaxEnergy;
    public float CurrentEnergy;
    public float MaxFuel;
    public float CurrentFuel;

    public RadarProfile(
        int team,
        Vector2 position,
        float maxShield,
        float currentShield,
        float maxHP,
        float currentHP,
        float maxEnergy,
        float currentEnergy,
        float maxFuel,
        float currentFuel)
    {
        Team = team;
        Position = position;
        MaxShield = maxShield;
        CurrentShield = currentShield;
        MaxHP = maxHP;
        CurrentHP = currentHP;
        MaxEnergy = maxEnergy;
        CurrentEnergy = currentEnergy;
        MaxFuel = maxFuel;
        CurrentFuel = currentFuel;
    }
}
