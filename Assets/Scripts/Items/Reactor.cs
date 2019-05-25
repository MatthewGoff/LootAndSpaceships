public class Reactor : Item
{
    public float EnergyCapacity;
    public float CurrentEnergy;
    public float EnergyRegen;
    public float FuelCapacity;
    public float CurrentFuel;
    public float FuelConsumption;

    public Reactor(float weight, float volume, float energyCapacity, float energyRegen, float fuelCapacity, float fuelUsage): base(weight, volume, ItemClass.Reactor)
    {
        EnergyCapacity = energyCapacity;
        CurrentEnergy = EnergyCapacity;
        EnergyRegen = energyRegen;
        FuelCapacity = fuelCapacity;
        CurrentFuel = FuelCapacity;
        FuelConsumption = fuelUsage;
    }

    public Reactor(ReactorParameters parameters): base(parameters.Weight, parameters.Volume, ItemClass.Reactor)
    {
        EnergyCapacity = parameters.EnergyCapacity;
        CurrentEnergy = EnergyCapacity;
        EnergyRegen = parameters.EnergyRegen;
        FuelCapacity = parameters.FuelCapacity;
        CurrentFuel = FuelCapacity;
        FuelConsumption = parameters.FuelConsumption;
    }
}
