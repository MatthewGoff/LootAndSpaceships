using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlotsamController : MonoBehaviour
{
    private readonly float MAX_INITIAL_VELOCITY = 5f;
    private readonly float ACCELERATION_TOWARDS_PLAYER = 0.3f;
    private readonly float PLAYER_ATTRACTION_RADIUS = 5f;

    private Vector2 InitialVelocity;
    private float SpeedTowardsPlayer;
    private Vector2 VelocityTowardsPlayer;

    protected virtual void Start()
    {
        InitialVelocity = MAX_INITIAL_VELOCITY * Random.insideUnitCircle;
        SpeedTowardsPlayer = 0f;
        VelocityTowardsPlayer = Vector2.zero;
    }

    protected virtual void FixedUpdate()
    {
        InitialVelocity *= Mathf.Pow(0.5f, Time.fixedDeltaTime);
        SpeedTowardsPlayer *= Mathf.Pow(0.5f, Time.fixedDeltaTime);
        VelocityTowardsPlayer *= Mathf.Pow(0.5f, Time.fixedDeltaTime);
        ApplyPlayerAttraction();
        Vector2 velocity = InitialVelocity + VelocityTowardsPlayer;
        transform.position += (Vector3)velocity * Time.fixedDeltaTime;
    }

    private void ApplyPlayerAttraction()
    {
        Dictionary<int, RadarProfile> radarProfiles = RadarOmniscience.Instance.PingRadar();
        foreach (RadarProfile radarProfile in radarProfiles.Values)
        {
            if (radarProfile.Team == 0)
            {
                Vector2 direction = radarProfile.Position - (Vector2)transform.position;
                if (direction.magnitude <= PLAYER_ATTRACTION_RADIUS)
                {
                    SpeedTowardsPlayer += ACCELERATION_TOWARDS_PLAYER;
                    VelocityTowardsPlayer = direction.normalized * SpeedTowardsPlayer;
                }
            }
        }
    }
}
