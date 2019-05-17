using UnityEngine;

public abstract class Autopilot
{
    private static readonly float TARGET_RADIUS = 0.5f;

    protected VehicleController VehicleController;

    protected AutopilotBehaviour Behaviour;
    public Vector2 Target;

    public bool OnStandby;

    protected Autopilot(VehicleController vehicleController)
    {
        VehicleController = vehicleController;
        Standby();
    }

    public void Standby()
    {
        Behaviour = AutopilotBehaviour.Standby;
        OnStandby = true;
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
            if (AtTarget() && VehicleController.Velocity == Vector2.zero)
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
        return (VehicleController.Position - Target).magnitude < TARGET_RADIUS;
    }
}