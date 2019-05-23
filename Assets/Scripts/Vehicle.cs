using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    public float MaximumSpeed { get; protected set; }
    public float ThrustForce { get; protected set; }
    public float TurnForce { get; private set; }

    protected Rigidbody2D RB2D;

    public float Radius
    {
        get
        {
            return transform.localScale.x;
        }
    }
    public float AngularVelocity
    {
        get
        {
            return 5 * AngularAcceleration;
        }
    }
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
    public float Mass
    {
        get
        {
            return RB2D.mass;
        }
    }
    public float Speed
    {
        get
        {
            return Velocity.magnitude;
        }
    }
    public float RotationalInertia
    {
        get
        {
            return (1f / 2f) * Mass * Mathf.Pow(Radius, 2);
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
    public float Torque
    {
        get
        {
            return TurnForce * Radius;
        }
    }
    public float AngularAcceleration
    {
        get
        {
            return Torque / RotationalInertia;
        }
    }

    public bool ThrustInput;
    public bool BreakInput;
    public float TurnInput;

    protected void Initialize(float thrustForce, float turnForce, float maximumSpeed, float mass)
    {
        ThrustForce = thrustForce;
        TurnForce = turnForce;
        MaximumSpeed = maximumSpeed;
        RB2D = GetComponent<Rigidbody2D>();
        RB2D.mass = mass;
    }

    protected bool UpdateVehicle(bool energyAvailable)
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
            RB2D.rotation += TurnInput * AngularVelocity * Time.fixedDeltaTime;
        }

        if (RB2D.velocity.magnitude > MaximumSpeed)
        {
            RB2D.velocity = RB2D.velocity.normalized * MaximumSpeed;
        }
        return energyConsumed;
    }

    protected void ApplyRecoil(Vector2 recoil)
    {
        RB2D.AddForce(recoil, ForceMode2D.Impulse);
    }
}