using UnityEngine;

public class FastAutopilotDirected : Autopilot
{
    public FastAutopilotDirected(VehicleController vehicle) : base(vehicle) { }

    protected override void SeekUpdate()
    {
        GetVehicleController().TurnInput = 0;
        GetVehicleController().ThrustInput = false;
        GetVehicleController().BreakInput = false;

        Vector2 targetVector = Target - GetVehicleController().Position;
        Vector2 perpendicularTargetVector = new Vector2(targetVector.y, -targetVector.x);
        Vector2 parallelVelocity = targetVector * Vector2.Dot(targetVector, GetVehicleController().Velocity) / Mathf.Pow(targetVector.magnitude, 2f);
        Vector2 perpendicularVelocity = GetVehicleController().Velocity - parallelVelocity;
        Vector2 perpendicularNextVelocity = perpendicularTargetVector * Vector2.Dot(perpendicularTargetVector, GetVehicleController().Velocity) / Mathf.Pow(perpendicularTargetVector.magnitude, 2f);
        float headingAngle = Vector2.SignedAngle(targetVector, GetVehicleController().HeadingVector);
        float perpendicularAngle = Vector2.SignedAngle(targetVector, perpendicularVelocity);

        if (Vector2.Angle(targetVector, GetVehicleController().Velocity) > 45f)
        {
            GetVehicleController().BreakInput = true;
        }
        if (Mathf.Abs(headingAngle) < 45f
            && Vector2.Dot(GetVehicleController().HeadingVector, GetVehicleController().Velocity) < 0f
            && Vector2.Dot(perpendicularNextVelocity, perpendicularVelocity) >= 0f)
        {
            GetVehicleController().ThrustInput = true;
        }
        if (Mathf.Abs(headingAngle) < 90f
            && Vector2.Dot(perpendicularVelocity, GetVehicleController().HeadingVector) < 0f
            && Vector2.Dot(perpendicularNextVelocity, perpendicularVelocity) >= 0f)
        {
            GetVehicleController().ThrustInput = true;
        }

        if (Mathf.Abs(headingAngle) < 1f)
        {
            if (GetVehicleController().Speed < GetVehicleController().MaximumSpeed)
            {
                GetVehicleController().ThrustInput = true;
            }
            else
            {
                GetVehicleController().ThrustInput = false;
            }
        }

        float turnRate = GetVehicleController().TurnRate * (2f * Mathf.PI / 360f);
        float thrustDuration = perpendicularVelocity.magnitude / GetVehicleController().Acceleration;
        float desiredHeadingAngle;
        if (turnRate * thrustDuration > 1f)
        {
            desiredHeadingAngle = 90f;
        }
        else
        {
            desiredHeadingAngle = (360f / (2f * Mathf.PI)) * Mathf.Acos(1 - turnRate * thrustDuration);
        }
        if (perpendicularAngle > 0f)
        {
            desiredHeadingAngle *= -1;
        }

        if (Mathf.Abs(desiredHeadingAngle) < 15f)
        {
            desiredHeadingAngle = 0f;
        }

        float desiredHeadingChange = desiredHeadingAngle - headingAngle;
        GetVehicleController().TurnInput = CalculateTurnInput(desiredHeadingChange);
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
                float headingAngle = Vector2.SignedAngle(targetVector, GetVehicleController().HeadingVector);
                if (Mathf.Abs(headingAngle) > 179f)
                {
                    GetVehicleController().ThrustInput = true;
                }
                else
                {
                    float desiredHeadingChange = 180 - headingAngle;
                    GetVehicleController().TurnInput = CalculateTurnInput(desiredHeadingChange);
                }
            }
        }
        
        if (targetVector.magnitude < 1f)
        {
            GetVehicleController().TurnInput = 0;
        }
    }

    private float CalculateTurnInput(float desiredHeadingChange)
    {
        if (desiredHeadingChange < -180f)
        {
            desiredHeadingChange += 360f;
        }
        else if (desiredHeadingChange > 180f)
        {
            desiredHeadingChange -= 360f;
        }

        float turnInput = Mathf.Clamp(desiredHeadingChange, -GetVehicleController().TurnRate * Time.fixedDeltaTime, GetVehicleController().TurnRate * Time.fixedDeltaTime);
        turnInput /= GetVehicleController().TurnRate * Time.fixedDeltaTime;
        return turnInput;
    }

    private DirectedVehicleController GetVehicleController()
    {
        return (DirectedVehicleController)VehicleController;
    }
}