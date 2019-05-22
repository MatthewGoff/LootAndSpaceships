using System;
using System.Collections.Generic;
using UnityEngine;

public static class SpaceshipTable
{
    private static readonly string SPACESHIP_TABLE_PATH = Application.dataPath + "/../SpaceshipTable.ods";

    public static SpaceshipParameters PrepareSpaceshipParameters(SpaceshipModel model, string name, int team)
    {
        string[,] table = OpenTable();
        int column = GetColumn(table, model);
        SpaceshipParameters parameters = new SpaceshipParameters
        {
            Model = model,
            Name = name,
            Team = team,
            VehicleType = Helpers.ParseVehicleType(table[column, GetRow(table, "Vehicle Type")]),
            TargetingType = Helpers.ParseTargetingType(table[column, GetRow(table, "Targeting Type")]),
            Size = Helpers.ParseFloat(table[column, GetRow(table, "Size")]),
            MassMultiplier = Helpers.ParseFloat(table[column, GetRow(table, "Mass Multiplier")]),
            ThrustForce = Helpers.ParseFloat(table[column, GetRow(table, "Thrust Force")]),
            TurnRate = Helpers.ParseFloat(table[column, GetRow(table, "Turn Rate")]),
            MaximumSpeed = Helpers.ParseFloat(table[column, GetRow(table, "Maximum Speed")]),
            AIType = Helpers.ParseAIType(table[column, GetRow(table, "AI Type")]),
            AIParameters = new string[]
            {
                table[column, GetRow(table, "AI Param 1")],
                table[column, GetRow(table, "AI Param 2")],
                table[column, GetRow(table, "AI Param 3")],
                table[column, GetRow(table, "AI Param 4")],
                table[column, GetRow(table, "AI Param 5")]
            },
            BurnDuration = Helpers.ParseFloat(table[column, GetRow(table, "Burn Duration")]),
            MaxShield = Helpers.ParseFloat(table[column, GetRow(table, "Maximum Shield")]),
            ShieldRegen = Helpers.ParseFloat(table[column, GetRow(table, "Shield Regen")]),
            ShieldEnergy = Helpers.ParseFloat(table[column, GetRow(table, "Shield Energy")]),
            MaxHealth = Helpers.ParseFloat(table[column, GetRow(table, "Maximum Health")]),
            HealthRegen = Helpers.ParseFloat(table[column, GetRow(table, "Health Regen")]),
            MaxEnergy = Helpers.ParseFloat(table[column, GetRow(table, "Maximum Energy")]),
            EnergyRegen = Helpers.ParseFloat(table[column, GetRow(table, "Energy Regen")]),
            MaxFuel = Helpers.ParseFloat(table[column, GetRow(table, "Maximum Fuel")]),
            HullSpaceMultiplier = Helpers.ParseFloat(table[column, GetRow(table, "Hull Space Multiplier")]),
            FuelUsage = Helpers.ParseFloat(table[column, GetRow(table, "Fuel Usage")]),
            AttackEnergy = Helpers.ParseFloat(table[column, GetRow(table, "Attack Energy")]),
            ThrustEnergy = Helpers.ParseFloat(table[column, GetRow(table, "Thrust Energy")]),
            LifeSupportEnergy = Helpers.ParseFloat(table[column, GetRow(table, "Life Support Energy")]),
            LifeSupportDegen = Helpers.ParseFloat(table[column, GetRow(table, "Life Support Degen")]),
            AttackCooldown = Helpers.ParseFloat(table[column, GetRow(table, "Attack Cooldown")]),
            LootExperience = Helpers.ParseInt(table[column, GetRow(table, "Experience")]),
            LootCredits = Helpers.ParseInt(table[column, GetRow(table, "Credits")]),
            LootFuel = Helpers.ParseInt(table[column, GetRow(table, "Fuel")]),
            LootScrap = Helpers.ParseInt(table[column, GetRow(table, "Scrap")]),
            LootItems = Helpers.ParseInt(table[column, GetRow(table, "Items")]),
           };

        return parameters;
    }

    public static Dictionary<SpaceshipModel, int> GetSpawnNumbers(int level)
    {
        string[,] table = OpenTable();
        int row = GetRow(table, "Level " + level.ToString());

        Dictionary<SpaceshipModel, int> spawnNumbers = new Dictionary<SpaceshipModel, int>();
        foreach (SpaceshipModel model in Enum.GetValues(typeof(SpaceshipModel)))
        {
            spawnNumbers.Add(model, Helpers.ParseInt(table[GetColumn(table, model), row]));
        }
        return spawnNumbers;
    }

    private static string[,] OpenTable()
    {
        string xml = Helpers.XMLfromODS(SPACESHIP_TABLE_PATH);
        return Helpers.StringTablefromODSXML(xml);
    }

    private static int GetColumn(string[,] table, SpaceshipModel model)
    {
        for (int x = 0; x < table.GetLength(0); x++)
        {
            if (table[x, 0] == model.ToString())
            {
                return x;
            }
        }
        return -1;
    }

    private static int GetRow(string[,] table, string label)
    {
        for (int y = 0; y < table.GetLength(1); y++)
        {
            if (table[0, y] == label)
            {
                return y;
            }
        }
        Debug.LogError("Could not find row \"" + label + "\" in Spaceship Table");
        return -1;
    }
}
