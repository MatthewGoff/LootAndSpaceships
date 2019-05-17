using System.Collections.Generic;
using UnityEngine;

public class DirectedPlayerController : PlayerController
{
    public GameObject ExhaustEffect;

    public void Initialize(string name)
    {
        VehicleController vehicleController = new DirectedVehicleController(
            rb2d: GetComponent<Rigidbody2D>(),
            thrustForce: 10f,
            turnRate: 300f,
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

        float turnInput = -Input.GetAxis("Horizontal");
        bool thrustInput = Input.GetKey(KeyCode.W);
        bool breakInput = Input.GetKey(KeyCode.S);

        if (turnInput != 0f
            || thrustInput
            || breakInput)
        {
            DismissAutopilot();
        }

        if (!UsingAutopilot)
        {
            GetVehicleController().TurnInput = turnInput;
            GetVehicleController().ThrustInput = thrustInput;
            GetVehicleController().BreakInput = breakInput;
        }

        if (GetVehicleController().ThrustInput)
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Enable();
        }
        else
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Disable();
        }

    }

    private DirectedVehicleController GetVehicleController()
    {
        return (DirectedVehicleController)VehicleController;
    }
}
