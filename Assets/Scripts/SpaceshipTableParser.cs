using System;
using UnityEngine;

public static class SpaceshipTable
{
    private static readonly string SPACESHIP_TABLE_PATH = UnityEngine.Application.dataPath + "/../SpaceshipTable.ods";

    public static SpaceshipParameters GetModelParameters(string model)
    {
        string[,] table = OpenTable();
        int column = GetColumn(table, model);
        SpaceshipParameters parameters = new SpaceshipParameters();

        parameters.VehicleType = (VehicleType)Enum.Parse(typeof(VehicleType), table[column, GetRow(table, "Vehicle Type")]);
        parameters.TargetingType = (TargetingType)Enum.Parse(typeof(TargetingType), table[column, GetRow(table, "Targeting Type")]);
        parameters.ThrustForce = int.Parse(table[column, GetRow(table, "Thrust Force")]);
        parameters.TurnRate = int.Parse(table[column, GetRow(table, "Turn Rate")]);
        parameters.MaximumSpeed = int.Parse(table[column, GetRow(table, "Maximum Speed")]);
        parameters.AIType = (AIType)Enum.Parse(typeof(AIType), table[column, GetRow(table, "AI Type")]);
        parameters.AIParameters = new string[5];
        parameters.AIParameters[0] = table[column, GetRow(table, "AI Param 1")];
        parameters.AIParameters[1] = table[column, GetRow(table, "AI Param 2")];
        parameters.AIParameters[2] = table[column, GetRow(table, "AI Param 3")];
        parameters.AIParameters[3] = table[column, GetRow(table, "AI Param 4")];
        parameters.AIParameters[4] = table[column, GetRow(table, "AI Param 5")];
        parameters.BurnDuration = float.Parse(table[column, GetRow(table, "Burn Duration")]);
        parameters.MaxShield = float.Parse(table[column, GetRow(table, "Maximum Shield")]);
        parameters.ShieldRegen = float.Parse(table[column, GetRow(table, "Shield Regen")]);
        parameters.ShieldEnergy = float.Parse(table[column, GetRow(table, "Shield Energy")]);
        parameters.MaxHealth = float.Parse(table[column, GetRow(table, "Maximum Health")]);
        parameters.HealthRegen = float.Parse(table[column, GetRow(table, "Health Regen")]);
        parameters.MaxEnergy = float.Parse(table[column, GetRow(table, "Maximum Energy")]);
        parameters.EnergyRegen = float.Parse(table[column, GetRow(table, "Energy Regen")]);
        parameters.MaxFuel = float.Parse(table[column, GetRow(table, "Maximum Fuel")]);
        parameters.FuelUsage = float.Parse(table[column, GetRow(table, "Fuel Usage")]);
        parameters.AttackEnergy = float.Parse(table[column, GetRow(table, "Attack Energy")]);
        parameters.ThrustEnergy = float.Parse(table[column, GetRow(table, "Thrust Energy")]);
        parameters.LifeSupportEnergy = float.Parse(table[column, GetRow(table, "Life Support Energy")]);
        parameters.LifeSupportDegen = float.Parse(table[column, GetRow(table, "Life Support Degen")]);

        return parameters;
    }

    private static string[,] OpenTable()
    {
        string xml = Helpers.XMLfromODS(SPACESHIP_TABLE_PATH);
        return Helpers.StringTablefromODSXML(xml);
    }

    private static int GetColumn(string[,] table, string label)
    {
        for (int x = 0; x < table.GetLength(0); x++)
        {
            if (table[x, 0] == label)
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
