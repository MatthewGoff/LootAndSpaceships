public class ShieldGenerator : Item
{
    public float HitPoints;
    public float Regen;
    public float EnergyUsage;

    public ShieldGenerator(float weight, float volume, float hitPoints, float regen, float energyUsage) : base(weight, volume, ItemClass.ShieldGenerator)
    {
        HitPoints = hitPoints;
        Regen = regen;
        EnergyUsage = energyUsage;
    }
}
