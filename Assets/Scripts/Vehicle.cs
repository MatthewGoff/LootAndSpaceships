using UnityEngine;

public abstract class Vehicle : CombatantManager
{
    public float ThrustForce;
    public float TurnRate; // Degrees per Second
    public float MaximumSpeed;
    public Rigidbody2D RB2D;
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

    public Vector2 Heading;
    public bool ThrustInput;
    public bool BreakInput;
    public float TurnInput;
}