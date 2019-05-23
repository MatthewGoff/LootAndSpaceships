using UnityEngine;

public abstract class Autopilot
{
    private static readonly float TARGET_RADIUS = 0.5f;

    protected Vehicle Vehicle;

    protected AutopilotBehaviour Behaviour;
    public Vector2 Target;

    public bool OnStandby;

    protected Autopilot(Vehicle vehicle)
    {
        Vehicle = vehicle;
        Standby();
    }

    public void Standby()
    {
        Behaviour = AutopilotBehaviour.Standby;
        OnStandby = true;
    }

    public void Halt()
    {
        Behaviour = AutopilotBehaviour.Halt;
        OnStandby = false;
    }

    public void SetTarget(Vector2 target, AutopilotBehaviour behaviour)
    {
        if (behaviour == AutopilotBehaviour.Standby)
        {
            Standby();
        }
        else
        {
            Target = target;
            Behaviour = behaviour;
            OnStandby = false;
        }
    }

    public bool Update()
    {
        if (Behaviour == AutopilotBehaviour.Standby)
        {
            return true;
        }
        else if (Behaviour == AutopilotBehaviour.Arrive)
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
        else if (Behaviour == AutopilotBehaviour.Halt)
        {
            if (Vehicle.Velocity.magnitude != 0)
            {
                HaltUpdate();
                return false;
            }
            else
            {
                Standby();
                return true;
            }
        }
        else
        {
            Debug.LogWarning("Unknown autopilot behaviour");
            return false;
        }
    }

    protected abstract void SeekUpdate();

    protected abstract void ArriveUpdate();

    protected abstract void HaltUpdate();

    protected bool AtTarget()
    {
        return (Vehicle.Position - Target).magnitude < TARGET_RADIUS;
    }
}