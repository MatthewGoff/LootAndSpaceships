using UnityEngine;

public class PlayerController : CombatantManager
{
    public GameObject ExhaustGlow;
    public GameObject ExhaustParticles;

    private readonly float Acceleration = 1000f;
    private readonly float TurnRate = 360f; // Degrees per Second
    private readonly float MaximumSpeed = 30f;

    private Rigidbody2D RB2D;
    private Vector2 Heading;
    private int AttackType;

    private void Awake()
    {
        RB2D = GetComponent<Rigidbody2D>();
        Heading = new Vector2(1, 0);
        Team = 0;
        AttackType = 1;
    }

    private void Update()
    {
        float turnInput = -Input.GetAxis("Horizontal");
        Heading = Quaternion.Euler(new Vector3(0, 0, turnInput * Time.deltaTime * TurnRate)) * Heading;
        transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, Heading));
        if (Input.GetKey(KeyCode.W))
        {
            RB2D.AddForce(Heading.normalized * Acceleration * Time.deltaTime);
            ExhaustGlow.SetActive(true);
            ExhaustParticles.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            ExhaustGlow.SetActive(false);
            ExhaustParticles.GetComponent<ParticleSystem>().Stop();
        }
        if (Input.GetKey(KeyCode.S))
        {
            RB2D.AddForce(-RB2D.velocity.normalized * Acceleration * Time.deltaTime);
        }

        if (RB2D.velocity.magnitude > MaximumSpeed)
        {
            RB2D.velocity = RB2D.velocity.normalized * MaximumSpeed;
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

    public override Vector2 GetPosition()
    {
        return RB2D.position;
    }

    public override void RecieveHit(int damage, DamageType damageType)
    {

    }

}
