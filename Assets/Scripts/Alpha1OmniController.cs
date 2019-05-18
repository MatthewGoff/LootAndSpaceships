using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha1OmniController : Spaceship
{
    public GameObject Color1Sprite;
    public GameObject Color2Sprite;
    public GameObject ExhaustEffect;
    public GameObject Turret;

    public void Initialize(string name, AIType aiType, AttackType attackTypes, Spaceship mother, bool showFDN, int team)
    {
        VehicleController vehicleController = new OmnidirectionalVehicleController(
            rb2d: GetComponent<Rigidbody2D>(),
            thrustForce: 10f,
            maximumSpeed: 30f,
            mass: 1f
            );
        Autopilot autopilot = new FastAutopilotOmnidirectional(vehicleController);
        AI ai = AI.CreateAI(aiType, this, autopilot, attackTypes, mother);

        base.Initialize(
            autopilot: autopilot,
            ai: ai,
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

        ColorPalett colorPalett = ConfigurationColorPaletts.Instance.GetColorPalett("Team " + team);
        Color1Sprite.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(1);
        Color2Sprite.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(2);
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
