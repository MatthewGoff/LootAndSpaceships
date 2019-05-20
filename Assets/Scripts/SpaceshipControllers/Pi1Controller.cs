using UnityEngine;

public class Pi1Controller : Spaceship
{
    public GameObject SpriteColor1;
    public GameObject SpriteColor5;

    public void Initialize(Player player, string name, AIType aiType, AttackType attackTypes, Spaceship mother, bool showFDN, int team, TargetingType targetingType)
    {
        VehicleController vehicleController = new DirectedVehicleController(
            rb2d: GetComponent<Rigidbody2D>(),
            thrustForce: 20f,
            turnRate: 200f,
            maximumSpeed: 20f,
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
        SpriteColor1.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(1);
        SpriteColor5.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(5);
    }
}
