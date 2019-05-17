using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha1Controller : Spaceship
{
    public GameObject ExhaustEffect;

    public void Initialize(ControlType controlType, string name, bool showFDN, int team)
    {
        VehicleController vehicleController = new DirectedVehicleController(
            rb2d: GetComponent<Rigidbody2D>(),
            thrustForce: 10f,
            turnRate: 300f,
            maximumSpeed: 30f,
            mass: 1f
            );
        Autopilot autopilot = new FastAutopilotDirected(vehicleController);
        AI ai = null;
        if (controlType == ControlType.NPC)
        {
            ai = new PassiveAI(this, autopilot);
        }

        base.Initialize(
            controlType: controlType,
            autopilot : autopilot,
            ai : ai,
            showFDN: showFDN,
            vehicleController: vehicleController,
            team: team,
            burnDuration: 3f,
            maxShield: 250f,
            shieldRegen: 4f,
            shieldEnergy: 0.1f,
            maxHP: 500f,
            hpRegen: 1f,
            maxEnergy: 10f,
            energyRegen: 1f,
            maxFuel: 510,
            fuelUsage: 0.85f,
            maxHullSpace: 523.6f,
            name: name,
            attackEnergy: 0.5f,
            thrustEnergy: 1.5f,
            lifeSupportEnergy: 0.1f,
            lifeSupportDegen: 10f);
    }

    protected override void ModelSpecificUpdate()
    {
        if (Thrusting)
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Enable();
        }
        else
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Disable();
        }
    }
}
