﻿using UnityEngine;

public class Tau1Controller : Spaceship
{
    public GameObject SpriteColor1;
    public GameObject SpriteColor3;
    public GameObject SpriteColor5;

    public void Initialize(Player player, string name, AIType aiType, AttackType attackTypes, Spaceship mother, bool showFDN, int team, TargetingType targetingType)
    {
        VehicleController vehicleController = new DirectedVehicleController(
            rb2d: GetComponent<Rigidbody2D>(),
            thrustForce: 40f,
            turnRate: 400f,
            maximumSpeed: 40f,
            mass: Mathf.PI * Mathf.Pow(transform.localScale.x, 2)
            );
        Autopilot autopilot = new FastAutopilotDirected(vehicleController);
        AI ai = AI.CreateAI(aiType, this, autopilot, attackTypes, mother);

        base.Initialize(
            player: player,
            autopilot: autopilot,
            ai: ai,
            showFDN: showFDN,
            vehicleController: vehicleController,
            targetingType: targetingType,
            team: team,
            burnDuration: 3f,
            maxShield: 500f,
            shieldRegen: 4f,
            shieldEnergy: 0.1f,
            maxHP: 1000f,
            hpRegen: 1f,
            maxEnergy: 10f,
            energyRegen: 1f,
            maxFuel: 510,
            fuelUsage: 0.85f,
            maxHullSpace: 10f * Mathf.Pow(transform.localScale.x, 3),
            name: name,
            attackEnergy: 0.5f,
            thrustEnergy: 1.5f,
            lifeSupportEnergy: 0.1f,
            lifeSupportDegen: 10f);

        ColorPalett colorPalett = ConfigurationColorPaletts.Instance.GetColorPalett("Team " + team);
        SpriteColor1.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(1);
        SpriteColor3.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(3);
        SpriteColor5.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(5);
    }
}
