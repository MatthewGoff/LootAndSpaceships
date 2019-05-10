using UnityEngine;

public class EnemyController : Spaceship
{
    public GameObject ExhaustEffect;

    private Autopilot Autopilot;
    private AI AI;

    public void Initialize(string name)
    {
        base.Initialize(
            team: 1,
            thrustForce: 8f,
            turnRate: 300f,
            maximumSpeed: 25f,
            mass: 1f,
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

        Autopilot = new FastAutopilot(this);
        AI = new SimpleAI(this, Autopilot);
        ShowFDN = true;
    }

    private void Update()
    {
        AI.Update(RadarOmniscience.Instance.PingRadar(UID));
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
            GameManager.Instance.SelectTarget(UID);
        }
    }

    public override void TakeDamage(Combatant attacker, float damage, DamageType damageType)
    {
        base.TakeDamage(attacker, damage, damageType);
        AI.AlertDamage(attacker);
    }

    protected override void Die()
    {
        base.Die();
        base.Destroy();        
    }
}
