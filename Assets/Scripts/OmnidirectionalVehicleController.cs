using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmnidirectionalVehicleController : VehicleController
{

    public Vector2 ThrustInput;
    public bool BreakInput;

    public OmnidirectionalVehicleController(Rigidbody2D rb2d, float thrustForce, float maximumSpeed, float mass)
        : base(VehicleType.Omnidirectional, rb2d, thrustForce, maximumSpeed, mass)
    {

    }

    public override bool UpdateVehicle(bool energyAvailable)
    {
        bool energyConsumed = false;
        if (ThrustInput != Vector2.zero && energyAvailable)
        {
            RB2D.AddForce(ThrustInput.normalized * ThrustForce);
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

        if (RB2D.velocity.magnitude > MaximumSpeed)
        {
            RB2D.velocity = RB2D.velocity.normalized * MaximumSpeed;
        }
        return energyConsumed;
    }
}
