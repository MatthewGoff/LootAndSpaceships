using UnityEngine;

public class PlayerController : Vehicle
{
    public GameObject AutopilotTargetEffect;
    public GameObject ExhaustEffect;

    private Autopilot Autopilot;
    private bool UsingAutopilot;
    private int AttackType;

    private void Awake()
    {
        ThrustForce = 10f;
        TurnRate = 300f;
        MaximumSpeed = 30f;
        RB2D = GetComponent<Rigidbody2D>();
        RB2D.mass = 1f;
        Heading = new Vector2(1, 0);
        Team = 0;
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

        if (Input.GetMouseButton(1))
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
                RB2D.AddForce(-Heading * BulletAttackManager.Recoil, ForceMode2D.Impulse);
                int damage = Mathf.RoundToInt(Random.Range(60, 100));
                new BulletAttackManager(this, RB2D.position, Heading, RB2D.velocity, damage);
            }
            else if (AttackType == 2)
            {
                RB2D.AddForce(-Heading * RocketAttackManager.Recoil, ForceMode2D.Impulse);
                int damage = Mathf.RoundToInt(Random.Range(10f, 30f));
                new RocketAttackManager(this, GameManager.Instance.PlayerTarget, RB2D.position, Heading, RB2D.velocity, damage);
            }
            else
            {
                int damage = Mathf.RoundToInt(Random.Range(30f, 60f));
                new EMPAttackManager(this, RB2D.position, damage);
            }
        }
    }

    private void FixedUpdate()
    {
        if (ThrustInput)
        {
            RB2D.AddForce(Heading.normalized * ThrustForce);
        }
        if (BreakInput)
        {
            if (RB2D.velocity.magnitude < Acceleration * Time.fixedDeltaTime)
            {
                RB2D.velocity = Vector2.zero;
            }
            else
            {
                RB2D.AddForce(-RB2D.velocity.normalized * ThrustForce);
            }
        }
        if (TurnInput != 0)
        {
            Heading = Quaternion.Euler(new Vector3(0, 0, TurnInput * Time.fixedDeltaTime * TurnRate)) * Heading;
            transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, Heading));
        }

        if (RB2D.velocity.magnitude > MaximumSpeed)
        {
            RB2D.velocity = RB2D.velocity.normalized * MaximumSpeed;
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
