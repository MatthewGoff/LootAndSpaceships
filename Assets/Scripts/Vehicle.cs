﻿using UnityEngine;

public abstract class Vehicle : Combatant
{
    public float ThrustForce { get; protected set; }
    public float TurnRate { get; protected set; } // Degrees per Second
    public float MaximumSpeed { get; protected set; }
    protected Rigidbody2D RB2D;
    public float Acceleration
    {
        get
        {
            return ThrustForce / RB2D.mass;
        }
    }
    public Vector2 Position
    {
        get
        {
            return RB2D.position;
        }
    }
    public Vector2 Velocity
    {
        get
        {
            return RB2D.velocity;
        }
    }
    public float Speed
    {
        get
        {
            return Velocity.magnitude;
        }
    }
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

    public bool ThrustInput;
    public bool BreakInput;
    public float TurnInput;

    protected void Initialize(int team, float thrustForce, float turnRate, float maximumSpeed, float mass)
    {
        ThrustForce = thrustForce;
        TurnRate = turnRate;
        MaximumSpeed = maximumSpeed;
        RB2D = GetComponent<Rigidbody2D>();
        RB2D.mass = mass;
        base.Initialize(team);
    }

    protected void UpdateVehicle()
    {
        if (ThrustInput)
        {
            RB2D.AddForce(HeadingVector.normalized * ThrustForce);
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
            //= Quaternion.Euler(0, 0, TurnInput * Time.fixedDeltaTime * TurnRate) * Heading;
            //Transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, Heading));
        }

        if (RB2D.velocity.magnitude > MaximumSpeed)
        {
            RB2D.velocity = RB2D.velocity.normalized * MaximumSpeed;
        }
    }
}