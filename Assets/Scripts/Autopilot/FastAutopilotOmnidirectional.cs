using UnityEngine;

public class FastAutopilotOmnidirectional : Autopilot
{
    public FastAutopilotOmnidirectional(VehicleController vehicle) : base(vehicle) { }

    protected override void SeekUpdate()
    {
        GetVehicleController().ThrustInput = Vector2.zero;
        GetVehicleController().BreakInput = false;

        Vector2 targetVector = Target - GetVehicleController().Position;
        Vector2 perpendicularTargetVector = new Vector2(targetVector.y, -targetVector.x);
        Vector2 parallelVelocity = targetVector * Vector2.Dot(targetVector, GetVehicleController().Velocity) / Mathf.Pow(targetVector.magnitude, 2f);
        Vector2 perpendicularVelocity = GetVehicleController().Velocity - parallelVelocity;
        float perpendicularContribution = perpendicularVelocity.magnitude / (GetVehicleController().Acceleration * Time.fixedDeltaTime);
        perpendicularContribution /= 100f;
        perpendicularContribution = Mathf.Clamp01(perpendicularContribution);
        GetVehicleController().ThrustInput = perpendicularContribution * -perpendicularVelocity.normalized + (1 - perpendicularContribution) * targetVector.normalized;

        if (Vector2.Dot(GetVehicleController().ThrustInput, GetVehicleController().Velocity) < 0)
        {
            GetVehicleController().BreakInput = true;
        }
    }

    protected override void ArriveUpdate()
    {
        SeekUpdate();

        Vector2 targetVector = Target - GetVehicleController().Position;
        float velocityAngle = Vector2.SignedAngle(targetVector, GetVehicleController().Velocity);
        if (GetVehicleController().Velocity != Vector2.zero && Mathf.Abs(velocityAngle) < 1f)
        {
            float remainingFlightDuration = targetVector.magnitude / GetVehicleController().Velocity.magnitude;
            float requiredBreakDuration = GetVehicleController().Velocity.magnitude / (2 * GetVehicleController().Acceleration);
            if (remainingFlightDuration <= requiredBreakDuration)
            {
                GetVehicleController().BreakInput = true;
                GetVehicleController().ThrustInput = GetVehicleController().Velocity;
            }
        }
    }

    private OmnidirectionalVehicleController GetVehicleController()
    {
        return (OmnidirectionalVehicleController)VehicleController;
    }
}