using UnityEngine;

public class RadarProfile
{
    public int UID;
    public string Name;
    public int Team;
    public bool PlayerControlled;
    public Vector2 Position;
    public float MaxShield;
    public float CurrentShield;
    public float MaxHealth;
    public float CurrentHealth;
    public float MaxEnergy;
    public float CurrentEnergy;
    public float MaxFuel;
    public float CurrentFuel;

    public RadarProfile(
        int uid,
        string name,
        int team,
        bool playerControlled,
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
        UID = uid;
        Name = name;
        Team = team;
        PlayerControlled = playerControlled;
        Position = position;
        MaxShield = maxShield;
        CurrentShield = currentShield;
        MaxHealth = maxHP;
        CurrentHealth = currentHP;
        MaxEnergy = maxEnergy;
        CurrentEnergy = currentEnergy;
        MaxFuel = maxFuel;
        CurrentFuel = currentFuel;
    }
}
