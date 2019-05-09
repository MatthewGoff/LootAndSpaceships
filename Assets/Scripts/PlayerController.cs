using UnityEngine;

public class PlayerController : Spaceship
{
    public GameObject AutopilotTargetEffect;
    public GameObject ExhaustEffect;

    private Autopilot Autopilot;
    private bool UsingAutopilot;
    private int AttackType;

    public void Initialize(string name)
    {
        base.Initialize(
            team: 0,
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

        AttackType = 1;
        UsingAutopilot = false;
        Autopilot = new FastAutopilot(this);
        ShowFDN = false;
    }

    private void Update()
    {
        TurnInput = -Input.GetAxis("Horizontal");
        ThrustInput = Input.GetKey(KeyCode.W);
        BreakInput = Input.GetKey(KeyCode.S);

        if (TurnInput != 0f || ThrustInput || BreakInput)
        {
            DismissAutopilot();
        }

        if (Input.GetMouseButton(1) && !GameManager.MouseOverUI())
        {
            Vector2 target = MasterCameraController.GetMousePosition();
            Autopilot.SetTarget(target, AutopilotBehaviour.Seek);
            UsingAutopilot = true;
            AutopilotTargetEffect.SetActive(true);
            AutopilotTargetEffect.transform.position = MasterCameraController.GetMousePosition();
        }

        if (UsingAutopilot)
        {
            if (Autopilot.Update())
            {
                DismissAutopilot();
            }
        }

        if (ThrustInput)
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Enable();
        }
        else
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Disable();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AttackType = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AttackType = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AttackType = 3;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (AttackType == 1)
            {
                FireBullet = true;
            }
            else if (AttackType == 2)
            {
                FireRocket = true;
            }
            else if (AttackType == 3)
            {
                FireEMP = true;
            }
        }
    }
    
    private void DismissAutopilot()
    {
        UsingAutopilot = false;
        AutopilotTargetEffect.SetActive(false);
    }

    public override Vector2 GetPosition()
    {
        return Position;
    }

    public override void TakeDamage(Combatant attacker, float damage, DamageType damageType)
    {
        base.TakeDamage(attacker, damage, damageType);
    }
}
