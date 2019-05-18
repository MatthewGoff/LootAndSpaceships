﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryVehicleController : VehicleController
{
    public bool BreakInput;

    public StationaryVehicleController(Rigidbody2D rb2d, float thrustForce, float maximumSpeed, float mass)
        : base(VehicleType.Stationary, rb2d, thrustForce, maximumSpeed, mass)
    {

    }

    public override bool UpdateVehicle(bool energyAvailable)
    {
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
        return false;
    }
}
