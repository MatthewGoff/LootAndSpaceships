public class Engine : Item
{
    public float ThrustForceMultiplier;
    public float TurnForceMultiplier;
    public float MaximumSpeed;
    public float ThrustEnergy;

    public Engine(float weight, float volume, float thrustForceMultiplier, float turnForceMultiplier, float maximumSpeed, float thrustEnergy)
        : base(weight, volume, ItemClass.Engine, ItemType.Engine)
    {
        ThrustForceMultiplier = thrustForceMultiplier;
        TurnForceMultiplier = turnForceMultiplier;
        MaximumSpeed = maximumSpeed;
        ThrustEnergy = thrustEnergy;
    }

    public Engine(EngineParameters parameters) : base(parameters.Weight, parameters.Volume, ItemClass.Engine, ItemType.Engine)
    {
        ThrustForceMultiplier = parameters.ThrustForceMultiplier;
        TurnForceMultiplier = parameters.TurnForceMultiplier;
        MaximumSpeed = parameters.MaximumSpeed;
        ThrustEnergy = parameters.ThrustEnergy;
    }

    public static Engine CreateRandomEngine(int level)
    {
        return new Engine(0, 0, 0, 0, 0, 0);
    }
}
