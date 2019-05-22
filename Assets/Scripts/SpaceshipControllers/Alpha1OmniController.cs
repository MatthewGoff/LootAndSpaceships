using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha1OmniController : Spaceship
{
    public GameObject SpriteColor1;
    public GameObject SpriteColor5;
    public GameObject ExhaustEffect;
    public GameObject Turret;

    protected override void ModelSpecificInitialization()
    {
        ColorPalett colorPalett = ConfigurationColorPaletts.Instance.GetColorPalett("Team " + Team);
        SpriteColor1.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(1);
        SpriteColor5.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(5);
    }

    protected override void ModelSpecificUpdate()
    {
        if (Thrusting)
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Enable();
            FaceDirection(Vector2.SignedAngle(Vector2.right, ((OmnidirectionalVehicleController)VehicleController).ThrustInput));
        }
        else
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Disable();
            FaceDirection(Vector2.SignedAngle(Vector2.right, ((OmnidirectionalVehicleController)VehicleController).Velocity));
        }

        Vector2 turretTarget = MasterCameraController.GetMousePosition() - Position;
        float turretAngle = Vector2.SignedAngle(Vector2.right, turretTarget);
        Turret.transform.rotation = Quaternion.Euler(0, 0, turretAngle);
    }

    private void FaceDirection(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
