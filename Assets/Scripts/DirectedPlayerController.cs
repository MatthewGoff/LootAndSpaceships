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
        Autopilot autopilot = new FastAutopilotDirected(VehicleController); ;
        base.Initialize(
            name,
            vehicleController,
            autopilot);
    }

    protected override void Update()
    {
        base.Update();

        GetVehicleController().TurnInput = -Input.GetAxis("Horizontal");
        GetVehicleController().ThrustInput = Input.GetKey(KeyCode.W);
        GetVehicleController().BreakInput = Input.GetKey(KeyCode.S);

        if (GetVehicleController().TurnInput != 0f
            || GetVehicleController().ThrustInput
            || GetVehicleController().BreakInput)
        {
            DismissAutopilot();
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
