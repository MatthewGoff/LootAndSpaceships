using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectedVehicleController : VehicleController
{
    public float TurnRate { get; protected set; }

    public bool ThrustInput;
    public bool BreakInput;
    public float TurnInput;

    public Vector2 HeadingVector
    {
        get
        {
            return Quaternion.Euler(0, 0, RB2D.rotation) * Vector2.right;
        }
    }
    public float HeadingAngle
    {
        get
        {
            return RB2D.rotation;
        }
    }

    public DirectedVehicleController(Rigidbody2D rb2d, float thrustForce, float turnRate, float maximumSpeed, float mass)
        : base(VehicleType.Directed, rb2d, thrustForce, maximumSpeed, mass)
    {
        TurnRate = turnRate;
    }

    public override bool UpdateVehicle(bool energyAvailable)
    {
        bool energyConsumed = false;
        if (ThrustInput && energyAvailable)
        {
            RB2D.AddForce(HeadingVector.normalized * ThrustForce);
            energyConsumed = true;
        }
        if (BreakInput)
        {
            if (RB2D.velocity.magnitude < Acceleration * Time.fixedDeltaTime)
            {
                RB2D.velocity = Vector2.zero;
            }
            else
            {
                RB2D.AddForce(-RB2D.velocity.normalized * ThrustForce);
            }
        }
        if (TurnInput != 0)
        {
            RB2D.rotation += TurnInput * Time.fixedDeltaTime * TurnRate;
        }

        if (RB2D.velocity.magnitude > MaximumSpeed)
        {
            RB2D.velocity = RB2D.velocity.normalized * MaximumSpeed;
        }
        return energyConsumed;
    }
}
