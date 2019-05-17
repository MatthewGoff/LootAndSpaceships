using UnityEngine;

public class EnemyController : Spaceship
{
    public GameObject ExhaustEffect;

    private Autopilot Autopilot;
    private AI AI;

    public void Initialize(string name)
    {
        VehicleController vehicleController = new DirectedVehicleController(
            rb2d: GetComponent<Rigidbody2D>(),
            thrustForce: 10f,
            turnRate: 300f,
            maximumSpeed: 30f,
            mass: 1f
            );
        base.Initialize(
            vehicleController: vehicleController,
            team: 1,
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

        Autopilot = new FastAutopilotDirected(VehicleController);
        AI = new PassiveAI(this, Autopilot);
        ShowFDN = true;
    }

    private void Update()
    {
        AI.Update(RadarOmniscience.Instance.PingRadar(UID));
        Autopilot.Update();

        if (((DirectedVehicleController)VehicleController).ThrustInput)
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Enable();
        }
        else
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Disable();
        }
    }

    private void OnMouseDown()
    {
        if (!GameManager.MouseOverUI())
        {
            GameManager.Instance.SelectPlayerTarget(UID);
        }
    }

    public override void TakeDamage(AttackManager attackManager, float damage, DamageType damageType)
    {
        base.TakeDamage(attackManager, damage, damageType);
        if (attackManager != null)
        {
            AI.AlertDamage(attackManager.Attacker);
        }
    }

    protected override void Die()
    {
        DropLoot();
        base.Die();      
    }

    private void DropLoot()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(Prefabs.Instance.ExpMorsel, VehicleController.Position, Quaternion.identity);
            Instantiate(Prefabs.Instance.Coin, VehicleController.Position, Quaternion.identity);
            Instantiate(Prefabs.Instance.FuelRod, VehicleController.Position, Quaternion.identity);
            Instantiate(Prefabs.Instance.Scrap, VehicleController.Position, Quaternion.identity);
            Instantiate(Prefabs.Instance.Crate, VehicleController.Position, Quaternion.identity);
        }
    }
}
