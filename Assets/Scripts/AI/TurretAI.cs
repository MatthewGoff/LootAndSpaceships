using System.Collections.Generic;
using UnityEngine;

public class TurretAI : AI
{
    private static readonly float AGRO_DISTANCE = 10f;
    private static readonly float DEAGRO_DISTANCE = 20f;
    private static readonly float DEAGRO_TIME = 3f;

    private readonly Vector2 Home;

    private bool Aggroed;
    private float AggroCountdown;
    private AttackType AttackType;

    public TurretAI(Spaceship spaceship, Autopilot autopilot, AttackType attackType) : base(spaceship, autopilot)
    {
        Home = spaceship.Position;
        AttackType = attackType;
    }

    public override void Update(Dictionary<int, RadarProfile> radarProfiles)
    {
        int closestEnemyUID = SpaceshipRegistry.NULL_UID;
        float closestEnemyDistance = float.MaxValue;
        foreach (RadarProfile radarProfile in radarProfiles.Values)
        {
            if (radarProfile.Team != Spaceship.Team)
            {
                float distance = (Spaceship.Position - radarProfile.Position).magnitude;
                if (distance < closestEnemyDistance)
                {
                    closestEnemyUID = radarProfile.UID;
                    closestEnemyDistance = distance;
                }
            }
        }

        if (closestEnemyDistance < AGRO_DISTANCE)
        {
            Aggroed = true;
            AggroCountdown = DEAGRO_TIME;
        }
        else if (Aggroed == true && closestEnemyDistance > DEAGRO_DISTANCE)
        {
            AggroCountdown -= Time.deltaTime;
            if (AggroCountdown <= 0f)
            {
                Aggroed = false;
            }
        }

        if (Aggroed & closestEnemyUID != SpaceshipRegistry.NULL_UID)
        {
            Spaceship.SelectTarget(closestEnemyUID);
            Spaceship.QueueAttacks(AttackType);
        }
        else
        {
            Spaceship.DropTarget();
        }

        if (Spaceship.Velocity.magnitude > 0f)
        {
            Autopilot.Halt();
        }
    }

    public override void AlertDamage(Spaceship attacker)
    {
        Aggroed = true;
        AggroCountdown = DEAGRO_TIME;
    }
}
