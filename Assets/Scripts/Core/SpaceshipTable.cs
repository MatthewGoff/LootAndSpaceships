using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipTable
{
    private static readonly string SPACESHIP_TABLE_PATH = Application.dataPath + "/Configuration/SpaceshipTable.ods";
    public static SpaceshipTable Instance;

    private readonly string[,] Table;
    private readonly Dictionary<string, int> Configurations;
    private readonly Dictionary<string, int> Attributes;

    public static void Initialize()
    {
        Instance = new SpaceshipTable();
    }

    private SpaceshipTable()
    {
        Table = OpenTable();
        Configurations = new Dictionary<string, int>();
        for (int col = 1; col < Table.GetLength(0); col++)
        {
            if (Table[col, 0] == "")
            {
                break;
            }
            else
            {
                Configurations.Add(Table[col, 0], col);
            }
        }

        Attributes = new Dictionary<string, int>();
        string section = "";
        for (int row = 2; row < Table.GetLength(1); row++)
        {
            if (Table[0, row] == "")
            {
                section = Table[0, row + 1];
                row++;
                if (section == "")
                {
                    break;
                }
            }
            else
            {
                Attributes.Add(section + Table[0, row], row);
            }
        }
    }

    public Dictionary<string, int> GetSpawnQuantities(int level)
    {
        Dictionary<string, int> spawnNumbers = new Dictionary<string, int>();
        foreach (string configuration in Configurations.Keys)
        {
            spawnNumbers.Add(configuration, Helpers.ParseInt(Table[Configurations[configuration], Attributes["Spawn" + "Level " + level.ToString()]]));
        }
        return spawnNumbers;
    }

    public Dictionary<FlotsamType, int> GetLootDrops(string configuration)
    {
        Dictionary<FlotsamType, int> flotsamNumbers = new Dictionary<FlotsamType, int>();
        foreach (FlotsamType type in Enum.GetValues(typeof(FlotsamType)))
        {
            flotsamNumbers.Add(type, Helpers.ParseInt(Table[Configurations[configuration], Attributes["Loot" + type.ToString()]]));
        }
        return flotsamNumbers;
    }

    public AIParameters GetAIParameters(string configuration)
    {
        return new AIParameters()
        {
            AIType = Helpers.ParseAIType(Table[Configurations[configuration], Attributes["AI"+"AI Type"]]),
            Param1 = Table[Configurations[configuration], Attributes["AI" + "Param 1"]],
            Param2 = Table[Configurations[configuration], Attributes["AI" + "Param 2"]],
            Param3 = Table[Configurations[configuration], Attributes["AI" + "Param 3"]],
        };
    }

    public HullParameters GetHullParameters(string configuration)
    {
        return new HullParameters()
        {
            Model = Helpers.ParseHullModel(Table[Configurations[configuration], Attributes["Hull" + "Model"]]),
            Size = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Hull" + "Size"]]),
            MassMultiplier = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Hull" + "Mass Multiplier"]]),
            HullSpaceMultiplier = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Hull" + "Hull Space Multiplier"]]),
            HitPoints = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Hull" + "Hit Points"]]),
            TargetingType = Helpers.ParseTargetingType(Table[Configurations[configuration], Attributes["Hull" + "Targeting Type"]])
        };
    }

    public EngineParameters GetEngineParameters(string configuration)
    {
        return new EngineParameters()
        {
            Weight = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Engine" + "Weight"]]),
            Volume = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Engine" + "Volume"]]),
            ThrustForceMultiplier = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Engine" + "Thrust Force Multiplier"]]),
            TurnForceMultiplier = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Engine" + "Turn Force Multiplier"]]),
            MaximumSpeed = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Engine" + "Maximum Speed"]]),
            ThrustEnergy = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Engine" + "Thrust Energy"]])
        };
    }

    public ShieldGeneratorParameters GetShieldGeneratorParameters(string configuration)
    {
        return new ShieldGeneratorParameters()
        {
            Weight = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Shield Generator" + "Weight"]]),
            Volume = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Shield Generator" + "Volume"]]),
            HitPoints = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Shield Generator" + "Hit Points"]]),
            Regen = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Shield Generator" + "Regen"]]),
            EnergyUsage = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Shield Generator" + "Energy Usage"]]),
        };
    }

    public ReactorParameters GetReactorParameters(string configuration)
    {
        return new ReactorParameters()
        {
            Weight = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Reactor" + "Weight"]]),
            Volume = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Reactor" + "Volume"]]),
            EnergyCapacity = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Reactor" + "Energy Capacity"]]),
            EnergyRegen = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Reactor" + "Energy Regen"]]),
            FuelCapacity = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Reactor" + "Fuel Capacity"]]),
            FuelConsumption = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Reactor" + "Fuel Consumption"]])
        };
    }

    public LifeSupportParameters GetLifeSupportParameters(string configuration)
    {
        return new LifeSupportParameters()
        {
            Weight = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Life Support" + "Weight"]]),
            Volume = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Life Support" + "Volume"]]),
            EnergyUsage = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Life Support" + "Energy Usage"]]),
            Degen = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Life Support" + "Degen"]]),
            BurnDuration = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Life Support" + "Burn Duration"]]),
        };
    }

    public WeaponParameters GetWeaponParameters(string configuration, int slot)
    {
        return new WeaponParameters()
        {
            Weight = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Weight"]]),
            Volume = Helpers.ParseFloat(Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Volume"]]),
            WeaponType = Helpers.ParseWeaponType(Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Type"]]),
            Param1 = Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Param 1"]],
            Param2 = Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Param 2"]],
            Param3 = Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Param 3"]],
            Param4 = Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Param 4"]],
            Param5 = Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Param 5"]],
            Param6 = Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Param 6"]],
            Param7 = Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Param 7"]],
            Param8 = Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Param 8"]],
            Param9 = Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Param 9"]],
            Param10 = Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Param 10"]],
            Param11 = Table[Configurations[configuration], Attributes["Weapon " + slot.ToString() + "Param 11"]],
        };
    }

    public WeaponParameters[] GetWeaponParameters(string configuration)
    {
        List<WeaponParameters> weaponParameters = new List<WeaponParameters>();
        for (int weaponSlot = 1; weaponSlot <= 8; weaponSlot++)
        {
            if (Table[Configurations[configuration], Attributes["Weapon " + weaponSlot.ToString() + "Type"]] == "")
            {
                break;
            }
            else
            {
                weaponParameters.Add(GetWeaponParameters(configuration, weaponSlot));
            }
        }
        return weaponParameters.ToArray();
    }

    private string[,] OpenTable()
    {
        string xml = Helpers.XMLfromODS(SPACESHIP_TABLE_PATH);
        return Helpers.StringTablefromODSXML(xml);
    }
}
