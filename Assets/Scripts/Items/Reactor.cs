public class Reactor : Item
{
    public float EnergyCacheSize;
    public float EnergyRegen;
    public float FuelCapacity;
    public float FuelUsage;

    public Reactor(float weight, float volume, float energyCacheSize, float energyRegen, float fuelCapacity, float fuelUsage): base(weight, volume, ItemClass.Reactor)
    {
        EnergyCacheSize = energyCacheSize;
        EnergyRegen = energyRegen;
        FuelCapacity = fuelCapacity;
        FuelUsage = fuelUsage;
    }
}
