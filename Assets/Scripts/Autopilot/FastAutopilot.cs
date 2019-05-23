using UnityEngine;

public class FastAutopilot : Autopilot
{
    public FastAutopilot(Vehicle vehicle) : base(vehicle) { }

    protected override void SeekUpdate()
    {
        Vehicle.TurnInput = 0;
        Vehicle.ThrustInput = false;
        Vehicle.BreakInput = false;

        Vector2 targetVector = Target - Vehicle.Position;
        Vector2 perpendicularTargetVector = new Vector2(targetVector.y, -targetVector.x);
        Vector2 parallelVelocity = targetVector * Vector2.Dot(targetVector, Vehicle.Velocity) / Mathf.Pow(targetVector.magnitude, 2f);
        Vector2 perpendicularVelocity = Vehicle.Velocity - parallelVelocity;
        Vector2 perpendicularNextVelocity = perpendicularTargetVector * Vector2.Dot(perpendicularTargetVector, Vehicle.Velocity) / Mathf.Pow(perpendicularTargetVector.magnitude, 2f);
        float headingAngle = Vector2.SignedAngle(targetVector, Vehicle.HeadingVector);
        float perpendicularAngle = Vector2.SignedAngle(targetVector, perpendicularVelocity);

        if (Vector2.Angle(targetVector, Vehicle.Velocity) > 45f)
        {
            Vehicle.BreakInput = true;
        }
        if (Mathf.Abs(headingAngle) < 45f
            && Vector2.Dot(Vehicle.HeadingVector, Vehicle.Velocity) < 0f
            && Vector2.Dot(perpendicularNextVelocity, perpendicularVelocity) >= 0f)
        {
            Vehicle.ThrustInput = true;
        }
        if (Mathf.Abs(headingAngle) < 90f
            && Vector2.Dot(perpendicularVelocity, Vehicle.HeadingVector) < 0f
            && Vector2.Dot(perpendicularNextVelocity, perpendicularVelocity) >= 0f)
        {
            Vehicle.ThrustInput = true;
        }

        if (Mathf.Abs(headingAngle) < 1f)
        {
            if (Vehicle.Speed < Vehicle.MaximumSpeed)
            {
                Vehicle.ThrustInput = true;
            }
            else
            {
                Vehicle.ThrustInput = false;
            }
        }

        float turnRate = Vehicle.TurnForce * (2f * Mathf.PI / 360f);
        float thrustDuration = perpendicularVelocity.magnitude / Vehicle.Acceleration;
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
        Vehicle.TurnInput = CalculateTurnInput(desiredHeadingChange);
    }

    protected override void ArriveUpdate()
    {
        SeekUpdate();

        Vector2 targetVector = Target - Vehicle.Position;
        float velocityAngle = Vector2.SignedAngle(targetVector, Vehicle.Velocity);
        if (Vehicle.Velocity != Vector2.zero && Mathf.Abs(velocityAngle) < 1f)
        {
            float remainingFlightDuration = targetVector.magnitude / Vehicle.Velocity.magnitude;
            float requiredBreakDuration = Vehicle.Velocity.magnitude / (2 * Vehicle.Acceleration);
            if (remainingFlightDuration <= requiredBreakDuration)
            {
                Vehicle.BreakInput = true;
                float headingAngle = Vector2.SignedAngle(targetVector, Vehicle.HeadingVector);
                if (Mathf.Abs(headingAngle) > 179f)
                {
                    Vehicle.ThrustInput = true;
                }
                else
                {
                    float desiredHeadingChange = 180 - headingAngle;
                    Vehicle.TurnInput = CalculateTurnInput(desiredHeadingChange);
                }
            }
        }
        
        if (targetVector.magnitude < 1f)
        {
            Vehicle.TurnInput = 0;
        }
    }

    protected override void HaltUpdate()
    {
        Vehicle.TurnInput = 0;
        Vehicle.ThrustInput = false;
        Vehicle.BreakInput = true;
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

        float turnInput = Mathf.Clamp(desiredHeadingChange, -Vehicle.TurnForce * Time.fixedDeltaTime, Vehicle.TurnForce * Time.fixedDeltaTime);
        turnInput /= Vehicle.TurnForce * Time.fixedDeltaTime;
        return turnInput;
    }
}