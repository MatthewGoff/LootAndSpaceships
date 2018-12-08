using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private readonly float Acceleration = 1000f;
    private readonly float TurnRate = 360f; // Degrees per Second
    private readonly float MaximumSpeed = 30f;

    private Rigidbody2D RB2D;
    private Vector2 Heading;

    private void Awake()
    {
        RB2D = GetComponent<Rigidbody2D>();
        Heading = new Vector2(1, 0);
    }

    private void FixedUpdate()
    {
        float turnInput = -Input.GetAxis("Horizontal");
        Heading = Quaternion.Euler(new Vector3(0, 0, turnInput * Time.deltaTime * TurnRate)) * Heading;
        transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(new Vector2(1, 0), Heading));
        if (Input.GetKey(KeyCode.W))
        {
            RB2D.AddForce(Heading.normalized * Acceleration * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            RB2D.AddForce(-RB2D.velocity.normalized * Acceleration * Time.deltaTime);
        }

        if (RB2D.velocity.magnitude > MaximumSpeed)
        {
            RB2D.velocity = RB2D.velocity.normalized * MaximumSpeed;
        }
    }
}
