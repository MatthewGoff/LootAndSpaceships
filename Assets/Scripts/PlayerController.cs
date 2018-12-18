using UnityEngine;

public class PlayerController : CombatantManager
{
    public GameObject AutopilotTargetEffect;
    public GameObject ExhaustEffect;

    private readonly float ThrustForce = 15f;
    private readonly float TurnRate = 360f; // Degrees per Second
    private readonly float MaximumSpeed = 30f;
    private readonly float Mass = 1f;
    private float Acceleration
    {
        get
        {
            return ThrustForce / Mass;
        }
    }

    private Rigidbody2D RB2D;
    private Vector2 Heading;
    private Vector2 AutopilotTarget;
    private bool AutopilotActive;
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
        AutopilotActive = false;
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
            AutopilotTarget = MasterCameraController.GetMousePosition();
            AutopilotActive = true;
            AutopilotTargetEffect.SetActive(true);
            AutopilotTargetEffect.transform.position = AutopilotTarget;
        }

        if (AutopilotActive)
        {
            RunAutopilot();   
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

    private void RunAutopilot()
    {
        TurnInput = 0;
        ThrustInput = false;
        BreakInput = false;

        Vector2 targetVector = AutopilotTarget - RB2D.position;
        Vector2 perpendicularTargetVector = new Vector2(targetVector.y, -targetVector.x);
        Vector2 parallelVelocity = targetVector * Vector2.Dot(targetVector, RB2D.velocity) / Mathf.Pow(targetVector.magnitude, 2f);
        Vector2 perpendicularVelocity = RB2D.velocity - parallelVelocity;
        Vector2 nextVelocity = RB2D.velocity + (Heading.normalized * Acceleration * Time.fixedDeltaTime);
        Vector2 perpendicularNextVelocity = perpendicularTargetVector * Vector2.Dot(perpendicularTargetVector, RB2D.velocity) / Mathf.Pow(perpendicularTargetVector.magnitude, 2f);
        float headingAngle = Vector2.SignedAngle(targetVector, Heading);
        float perpendicularAngle = Vector2.SignedAngle(targetVector, perpendicularVelocity);

        if (Vector2.Angle(targetVector, RB2D.velocity) > 45f)
        {
            BreakInput = true;
        }
        if (Mathf.Abs(headingAngle) < 45f
            && Vector2.Dot(Heading, RB2D.velocity) < 0f)
        {
            ThrustInput = true;
        }
        if (Mathf.Abs(headingAngle) < 90f
            && Vector2.Dot(perpendicularVelocity, Heading) < 0f
            && Vector2.Dot(perpendicularNextVelocity, perpendicularVelocity) >= 0f)
        {
            ThrustInput = true;
        }

        if (Mathf.Abs(headingAngle) < 1f)
        {
            if (RB2D.velocity.magnitude < MaximumSpeed)
            {
                ThrustInput = true;
            }
            else
            {
                ThrustInput = false;
            }
        }

        float turnRate = TurnRate * (2f * Mathf.PI / 360f);
        float thrustDuration = perpendicularVelocity.magnitude / Acceleration;
        float desiredHeadingAngle;
        if (turnRate * thrustDuration > 1f)
        {
            desiredHeadingAngle = 90f;
        }
        else
        {
            desiredHeadingAngle = (360f / (2f * Mathf.PI)) * Mathf.Acos(1 - turnRate * thrustDuration);
        }
        if (perpendicularAngle > 0f)
        {
            desiredHeadingAngle *= -1;
        }

        if (Mathf.Abs(desiredHeadingAngle) < 15f)
        {
            desiredHeadingAngle = 0f;
        }

        TurnInput = desiredHeadingAngle - headingAngle;
        if (TurnInput < -180f)
        {
            TurnInput += 360f;
        }
        else if (TurnInput > 180f)
        {
            TurnInput -= 360f;
        }

        TurnInput = Mathf.Clamp(TurnInput, -TurnRate * Time.fixedDeltaTime, TurnRate * Time.fixedDeltaTime);
        TurnInput /= TurnRate * Time.fixedDeltaTime;
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

    private void DismissMoveTarget()
    {
        AutopilotActive = false;
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
