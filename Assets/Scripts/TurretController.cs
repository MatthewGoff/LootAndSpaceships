using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : Spaceship
{
    public GameObject Color1Sprite;
    public GameObject Color2Sprite;

    public void Initialize(string name, AIType aiType, AttackType attackTypes, Spaceship mother, bool showFDN, int team, TargetingType targetingType)
    {
        VehicleController vehicleController = new StationaryVehicleController(
            rb2d: GetComponent<Rigidbody2D>(),
            thrustForce: 10f,
            maximumSpeed: 30f,
            mass: 1f
            );
        Autopilot autopilot = new StationaryAutopilot(vehicleController);
        AI ai = AI.CreateAI(aiType, this, autopilot, attackTypes, mother);

        base.Initialize(
            null,
            autopilot: autopilot,
            ai: ai,
            showFDN: showFDN,
            vehicleController: vehicleController,
            targetingType: targetingType,
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

        ColorPalett colorPalett = ConfigurationColorPaletts.Instance.GetColorPalett("Team " + team);
        Color1Sprite.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(1);
        Color2Sprite.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(2);
    }
}
