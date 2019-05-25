public class ShieldGenerator : Item
{
    public float MaximumHitPoints;
    public float CurrentHitPoints;
    public float Regen;
    public float EnergyUsage;

    public ShieldGenerator(float weight, float volume, float hitPoints, float regen, float energyUsage) : base(weight, volume, ItemClass.ShieldGenerator)
    {
        MaximumHitPoints = hitPoints;
        CurrentHitPoints = MaximumHitPoints;
        Regen = regen;
        EnergyUsage = energyUsage;
    }

    public ShieldGenerator(ShieldGeneratorParameters parameters) : base(parameters.Weight, parameters.Volume, ItemClass.ShieldGenerator)
    {
        MaximumHitPoints = parameters.HitPoints;
        CurrentHitPoints = MaximumHitPoints;
        Regen = parameters.Regen;
        EnergyUsage = parameters.EnergyUsage;
    }
}
