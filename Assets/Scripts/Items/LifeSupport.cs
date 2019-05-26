public class LifeSupport : Item
{
    public float EnergyUsage;
    public float Degen;
    public float BurnDuration;

    public LifeSupport(float weight, float volume, float energyUsage, float degen, float burnDuration)
        : base(weight, volume, ItemClass.LifeSupport, ItemType.LifeSupport)
    {
        EnergyUsage = energyUsage;
        Degen = degen;
        BurnDuration = burnDuration;
    }

    public LifeSupport(LifeSupportParameters parameters) : base(parameters.Weight, parameters.Volume, ItemClass.LifeSupport, ItemType.LifeSupport)
    {
        EnergyUsage = parameters.EnergyUsage;
        Degen = parameters.Degen;
        BurnDuration = parameters.BurnDuration;
    }

    public static LifeSupport CreateRandomLifeSupport(int level)
    {
        return new LifeSupport(0, 0, 0, 0, 0);
    }
}
