using UnityEngine;

public abstract class VehicleController
{
    public float ThrustForce { get; protected set; }
    public bool Thrusting { get; protected set; }
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
    public VehicleType VehicleType;

    public VehicleController(VehicleType vehicleType, Rigidbody2D rb2d, float thrustForce, float maximumSpeed, float mass)
    {
        VehicleType = vehicleType;
        ThrustForce = thrustForce;
        MaximumSpeed = maximumSpeed;
        RB2D = rb2d;
        RB2D.mass = mass;
    }

    public abstract bool UpdateVehicle(bool energyAvailable);

    public void ApplyRecoil(Vector2 recoil)
    {
        RB2D.AddForce(recoil, ForceMode2D.Impulse);
    }
}