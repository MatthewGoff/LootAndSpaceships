using UnityEngine;

public abstract class Autopilot
{
    private static readonly float TargetRadius = 0.5f;

    protected Vehicle Vehicle;

    protected AutopilotBehaviour Behaviour;
    protected ITargetable Target;

    protected Autopilot(Vehicle vehicle)
    {
        Vehicle = vehicle;
    }

    public void SetTarget(ITargetable target, AutopilotBehaviour behaviour)
    {
        Target = target;
        Behaviour = behaviour;
    }

    public bool Update()
    {
        if (Behaviour == AutopilotBehaviour.Arrive)
        {
            if (AtTarget() && Vehicle.Velocity == Vector2.zero)
            {
                return true;
            }
            else
            {
                ArriveUpdate();
                return false;
            }
        }
        else if (Behaviour == AutopilotBehaviour.Seek)
        {
            SeekUpdate();
            return AtTarget();
        }
        else
        {
            Debug.LogWarning("Unknown autopilot behaviour");
            return false;
        }
    }

    protected abstract void SeekUpdate();

    protected abstract void ArriveUpdate();

    protected bool AtTarget()
    {
        return (Vehicle.GetPosition() - Target.GetPosition()).magnitude < TargetRadius;
    }
}