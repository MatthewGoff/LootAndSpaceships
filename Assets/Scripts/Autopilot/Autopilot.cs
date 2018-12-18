using UnityEngine;

public abstract class Autopilot
{
    public Vector2 Target;

    protected Vehicle Vehicle;

    protected Autopilot(Vehicle vehicle)
    {
        Vehicle = vehicle;
    }

    public abstract void Evaluate();
}