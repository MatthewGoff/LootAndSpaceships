using UnityEngine;

public class EnemyController : Spaceship
{
    public GameObject PortraitCamera;
    public GameObject ExhaustEffect;

    private Autopilot Autopilot;
    private AI AI;

    public void Initialize(string name)
    {
        base.Initialize(
            team: 1,
            thrustForce: 10f,
            turnRate: 300f,
            maximumSpeed: 30f,
            mass: 1f,
            burnDuration: 3f,
            maxShield: 500f,
            shieldRegen: 30f,
            maxHP: 1000f,
            hpRegen: 5f,
            maxEnergy: 10f,
            energyRegen: 1f,
            maxFuel: 510,
            fuelUsage: 0.85f,
            maxHullSpace: 523.6f,
            name: name);
        Autopilot = new FastAutopilot(this);
        AI = new SimpleAI(this, Autopilot);
        ShowFDN = true;
        AttackEnergy = 0.5f;
        ThrustEnergy = 0.5f;
    }

    private void Update()
    {
        AI.Update(RadarOmniscience.Instance.PingRadar(RadarIdentifier));
        Autopilot.Update();

        if (ThrustInput)
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
            GameManager.Instance.ChangeTarget(this);
        }
    }

    public override void TakeDamage(Combatant attacker, float damage, DamageType damageType)
    {
        base.TakeDamage(attacker, damage, damageType);
        AI.AlertDamage(attacker);
    }
}
