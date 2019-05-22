using System;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : AI
{
    private static readonly float AGRO_DISTANCE = 10f;
    private static readonly float DEAGRO_DISTANCE = 20f;
    private static readonly float DEAGRO_TIME = 3f;
    private static readonly float ORBIT_DISTANCE = 5f;
    private static readonly float ORBIT_ANGULAR_SPEED = 60f;

    private float OrbitAngle;
    private bool Aggroed;
    private float AggroCountdown;
    private AttackType AttackType;
    private Spaceship Mother;

    public DroneAI(Spaceship spaceship, Autopilot autopilot, Spaceship mother, string[] parameters) : base(spaceship, autopilot)
    {
        Mother = mother;
        AttackType = (AttackType)Enum.Parse(typeof(AttackType), parameters[0]);
        OrbitAngle = 0f;
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
            Autopilot.SetTarget(radarProfiles[closestEnemyUID].Position, AutopilotBehaviour.Seek);
            Spaceship.SelectTarget(closestEnemyUID);
            Spaceship.QueueAttacks(AttackType);
        }
        else
        {
            Spaceship.DropTarget();
            if (Mother == null)
            {
                Autopilot.Halt();
            }
            else
            {
                OrbitAngle += ORBIT_ANGULAR_SPEED * Time.deltaTime;
                Vector2 displacement = ORBIT_DISTANCE * (Quaternion.Euler(0, 0, OrbitAngle) * Vector2.right);
                Autopilot.SetTarget(Mother.Position + displacement, AutopilotBehaviour.Seek);
            }
        }
    }

    public override void AlertDamage(Spaceship attacker)
    {
        Aggroed = true;
        AggroCountdown = DEAGRO_TIME;
    }
}
