﻿using UnityEngine;

public abstract class Autopilot
{
    private static readonly float TARGET_RADIUS = 0.5f;

    protected VehicleController Vehicle;

    protected AutopilotBehaviour Behaviour;
    protected Vector2 Target;

    protected Autopilot(VehicleController vehicle)
    {
        Vehicle = vehicle;
    }

    public void Standby()
    {
        Behaviour = AutopilotBehaviour.Standby;
    }

    public void SetTarget(Vector2 target, AutopilotBehaviour behaviour)
    {
        Target = target;
        Behaviour = behaviour;
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
        return (Vehicle.Position - Target).magnitude < TARGET_RADIUS;
    }
}