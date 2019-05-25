public class LifeSupport : Item
{
    public float EnergyUsage;
    public float Degen;
    public float BurnDuration;

    public LifeSupport(float weight, float volume, float energyUsage, float degen, float burnDuration) : base(weight, volume, ItemClass.LifeSupport)
    {
        EnergyUsage = energyUsage;
        Degen = degen;
        BurnDuration = burnDuration;
    }

    public LifeSupport(LifeSupportParameters parameters) : base(parameters.Weight, parameters.Volume, ItemClass.LifeSupport)
    {
        EnergyUsage = parameters.EnergyUsage;
        Degen = parameters.Degen;
        BurnDuration = parameters.BurnDuration;
    }
}
