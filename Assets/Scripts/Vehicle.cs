using UnityEngine;

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
        Heading = Quaternion.Euler(transform.eulerAngles) * Vector2.right;
        base.Initialize(radarType, team);
    }

    private void FixedUpdate()
    {
        if (ThrustInput)
        {
            RB2D.AddForce(Heading.normalized * ThrustForce);
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
            Heading = Quaternion.Euler(new Vector3(0, 0, TurnInput * Time.fixedDeltaTime * TurnRate)) * Heading;
            transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, Heading));
        }

        if (RB2D.velocity.magnitude > MaximumSpeed)
        {
            RB2D.velocity = RB2D.velocity.normalized * MaximumSpeed;
        }
    }

    public override Vector2 GetPosition()
    {
        return RB2D.position;
    }
}