using UnityEngine;

public class PlayerController : CombatantManager
{
    public GameObject MoveTargetEffect;
    public GameObject ExhaustEffect;

    private readonly float Acceleration = 1000f;
    private readonly float TurnRate = 360f; // Degrees per Second
    private readonly float MaximumSpeed = 30f;

    private Rigidbody2D RB2D;
    private Vector2 Heading;
    private Vector2 MoveTarget;
    private bool HasMoveTarget;
    private int AttackType;

    private bool ThrustInput;
    private bool BreakInput;
    private float TurnInput;

    private void Awake()
    {
        RB2D = GetComponent<Rigidbody2D>();
        Heading = new Vector2(1, 0);
        Team = 0;
        AttackType = 1;
        HasMoveTarget = false;
    }

    private void Update()
    {
        TurnInput = -Input.GetAxis("Horizontal");
        ThrustInput = Input.GetKey(KeyCode.W);
        BreakInput = Input.GetKey(KeyCode.S);

        if (TurnInput != 0f || ThrustInput || BreakInput)
        {
            DismissMoveTarget();
        }

        if (Input.GetMouseButton(1))
        {
            MoveTarget = MasterCameraController.GetMousePosition();
            HasMoveTarget = true;
            MoveTargetEffect.SetActive(true);
            MoveTargetEffect.transform.position = MoveTarget;
        }

        if (HasMoveTarget)
        {
            Vector2 targetBearing = MoveTarget - (Vector2)transform.position;
            Vector2 velocitySchedule = targetBearing.normalized - RB2D.velocity.normalized;
            float angle = Vector2.SignedAngle(Heading, velocitySchedule);
            angle = Mathf.Clamp(angle, -TurnRate * Time.deltaTime, TurnRate * Time.deltaTime);
            TurnInput = angle / (TurnRate * Time.deltaTime);
            ThrustInput = true;
            if (Vector2.Angle(RB2D.velocity, velocitySchedule) < 90f)
            {
                ThrustInput = true;
            }
            else
            {
                //BreakInput = true;
            }
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
                RB2D.AddForce(-Heading * BulletAttackManager.Recoil);
                int damage = Mathf.RoundToInt(Random.Range(60, 100));
                new BulletAttackManager(this, RB2D.position, Heading, RB2D.velocity, damage);
            }
            else if (AttackType == 2)
            {
                RB2D.AddForce(-Heading * RocketAttackManager.Recoil);
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
            RB2D.AddForce(Heading.normalized * Acceleration * Time.deltaTime);
        }
        if (BreakInput)
        {
            RB2D.AddForce(-RB2D.velocity.normalized * Acceleration * Time.deltaTime);
        }
        if (TurnInput != 0)
        {
            Heading = Quaternion.Euler(new Vector3(0, 0, TurnInput * Time.deltaTime * TurnRate)) * Heading;
            transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, Heading));
        }

        if (RB2D.velocity.magnitude > MaximumSpeed)
        {
            RB2D.velocity = RB2D.velocity.normalized * MaximumSpeed;
        }
    }

    private void DismissMoveTarget()
    {
        HasMoveTarget = false;
        MoveTargetEffect.SetActive(false);
    }

    public override Vector2 GetPosition()
    {
        return RB2D.position;
    }

    public override void RecieveHit(int damage, DamageType damageType)
    {

    }

}
