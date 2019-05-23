public class Engine : Item
{
    public float ThrustForce;
    public float TurnForce;
    public float MaximumSpeed;

    public Engine(float weight, float volume, float thrustForce, float turnForce, float maximumSpeed) : base(weight, volume, ItemClass.Engine)
    {
        ThrustForce = thrustForce;
        TurnForce = turnForce;
        MaximumSpeed = maximumSpeed;
    }
}
