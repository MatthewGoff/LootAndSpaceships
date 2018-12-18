using UnityEngine;

public class FastAutopilot : Autopilot
{
    public FastAutopilot(Vehicle vehicle) : base(vehicle) { }

    public override void Evaluate()
    {
        Vehicle.TurnInput = 0;
        Vehicle.ThrustInput = false;
        Vehicle.BreakInput = false;

        Vector2 targetVector = Target - Vehicle.Position;
        Vector2 perpendicularTargetVector = new Vector2(targetVector.y, -targetVector.x);
        Vector2 parallelVelocity = targetVector * Vector2.Dot(targetVector, Vehicle.Velocity) / Mathf.Pow(targetVector.magnitude, 2f);
        Vector2 perpendicularVelocity = Vehicle.Velocity - parallelVelocity;
        Vector2 nextVelocity = Vehicle.Velocity + (Vehicle.Heading.normalized * Vehicle.Acceleration * Time.fixedDeltaTime);
        Vector2 perpendicularNextVelocity = perpendicularTargetVector * Vector2.Dot(perpendicularTargetVector, Vehicle.Velocity) / Mathf.Pow(perpendicularTargetVector.magnitude, 2f);
        float headingAngle = Vector2.SignedAngle(targetVector, Vehicle.Heading);
        float perpendicularAngle = Vector2.SignedAngle(targetVector, perpendicularVelocity);

        if (Vector2.Angle(targetVector, Vehicle.Velocity) > 45f)
        {
            Vehicle.BreakInput = true;
        }
        if (Mathf.Abs(headingAngle) < 45f
            && Vector2.Dot(Vehicle.Heading, Vehicle.Velocity) < 0f
            && Vector2.Dot(perpendicularNextVelocity, perpendicularVelocity) >= 0f)
        {
            Vehicle.ThrustInput = true;
        }
        if (Mathf.Abs(headingAngle) < 90f
            && Vector2.Dot(perpendicularVelocity, Vehicle.Heading) < 0f
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

        float turnRate = Vehicle.TurnRate * (2f * Mathf.PI / 360f);
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

        Vehicle.TurnInput = desiredHeadingAngle - headingAngle;
        if (Vehicle.TurnInput < -180f)
        {
            Vehicle.TurnInput += 360f;
        }
        else if (Vehicle.TurnInput > 180f)
        {
            Vehicle.TurnInput -= 360f;
        }

        Vehicle.TurnInput = Mathf.Clamp(Vehicle.TurnInput, -Vehicle.TurnRate * Time.fixedDeltaTime, Vehicle.TurnRate * Time.fixedDeltaTime);
        Vehicle.TurnInput /= Vehicle.TurnRate * Time.fixedDeltaTime;
    }
}