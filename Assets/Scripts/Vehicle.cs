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

    public Vector2 Heading { get; protected set; }
    public bool ThrustInput;
    public bool BreakInput;
    public float TurnInput;

    protected void Initialize(RadarType radarType, int team, float thrustForce, float turnRate, float maximumSpeed, float mass)
    {
        ThrustForce = thrustForce;
        TurnRate = turnRate;
        MaximumSpeed = maximumSpeed;
        RB2D = GetComponent<Rigidbody2D>();
        RB2D.mass = mass;
        base.Initialize(radarType, team);
    }
}