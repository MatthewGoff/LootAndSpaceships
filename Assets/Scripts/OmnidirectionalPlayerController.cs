using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmnidirectionalPlayerController : PlayerController
{
    public GameObject ExhaustEffect;
    public GameObject Turret;

    public void Initialize(string name)
    {
        VehicleController vehicleController = new OmnidirectionalVehicleController(
            rb2d: GetComponent<Rigidbody2D>(),
            thrustForce: 10f,
            maximumSpeed: 30f,
            mass: 1f
            );
        Autopilot autopilot = new FastAutopilotOmnidirectional(vehicleController); ;
        base.Initialize(
            name,
            vehicleController,
            autopilot);
    }

    protected override void Update()
    {
        base.Update();

        Vector2 thrustInput = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            thrustInput += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            thrustInput += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            thrustInput += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            thrustInput += Vector2.right;
        }
        
        bool breakInput = Input.GetKey(KeyCode.F);

        if (thrustInput != Vector2.zero
            || breakInput)
        {
            DismissAutopilot();
        }

        if (!UsingAutopilot)
        {
            GetVehicleController().ThrustInput = thrustInput;
            GetVehicleController().BreakInput = breakInput;
        }

        if (GetVehicleController().ThrustInput != Vector2.zero)
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Enable();
            float headingAngle = Vector2.SignedAngle(Vector2.right, GetVehicleController().ThrustInput);
            transform.rotation = Quaternion.Euler(0, 0, headingAngle);
        }
        else
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Disable();
        }

        Vector2 turretTarget = MasterCameraController.GetMousePosition() - Position;
        float turretAngle = Vector2.SignedAngle(Vector2.right, turretTarget);
        Turret.transform.rotation = Quaternion.Euler(0, 0, turretAngle);
    }

    private OmnidirectionalVehicleController GetVehicleController()
    {
        return (OmnidirectionalVehicleController)VehicleController;
    }
}
