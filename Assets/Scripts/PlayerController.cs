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
            radarType: RadarType.Ally,
            team: 0,
            thrustForce: 10f,
            turnRate: 300f,
            maximumSpeed: 30f,
            mass: 1f,
            burnDuration: 3f,
            maxShield: 100f,
            shieldRegen: 1f,
            maxHP: 100f,
            hpRegen: 1f,
            maxEnergy: 10f,
            energyRegen: 1f,
            maxFuel: 510,
            fuelUsage: 0.85f,
            maxHullSpace: 523.6f,
            name: name);

        AttackType = 1;
        UsingAutopilot = false;
        Autopilot = new FastAutopilot(this);
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
            Autopilot.Target = MasterCameraController.GetMousePosition();
            UsingAutopilot = true;
            AutopilotTargetEffect.SetActive(true);
            AutopilotTargetEffect.transform.position = Autopilot.Target;
        }

        if (UsingAutopilot)
        {
            Autopilot.Evaluate();   
        }

        ExhaustEffect.SetActive(ThrustInput);

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
                int damage = Mathf.RoundToInt(Random.Range(60, 100));
                new BulletAttackManager(this, RB2D.position, Heading, RB2D.velocity, damage);
                RB2D.AddForce(-Heading * BulletAttackManager.Recoil, ForceMode2D.Impulse);
            }
            else if (AttackType == 2)
            {
                int damage = Mathf.RoundToInt(Random.Range(10f, 30f));
                new RocketAttackManager(this, GameManager.Instance.PlayerTarget, RB2D.position, Heading, RB2D.velocity, damage);
                RB2D.AddForce(-Heading * RocketAttackManager.Recoil, ForceMode2D.Impulse);
            }
            else
            {
                int damage = Mathf.RoundToInt(Random.Range(30f, 60f));
                new EMPAttackManager(this, RB2D.position, damage);
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
        return RB2D.position;
    }

    public override void RecieveHit(int damage, DamageType damageType)
    {

    }
}
