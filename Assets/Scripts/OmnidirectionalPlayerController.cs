using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmnidirectionalPlayerController : PlayerController
{
    public GameObject ExhaustEffect;

    public void Initialize(string name)
    {
        VehicleController vehicleController = new OmnidirectionalVehicleController(
            rb2d: GetComponent<Rigidbody2D>(),
            thrustForce: 10f,
            maximumSpeed: 30f,
            mass: 1f
            );
        Autopilot autopilot = new FastAutopilotDirected(vehicleController); ;
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
            || GetVehicleController().BreakInput)
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
        }
        else
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Disable();
        }

    }

    private OmnidirectionalVehicleController GetVehicleController()
    {
        return (OmnidirectionalVehicleController)VehicleController;
    }
}
