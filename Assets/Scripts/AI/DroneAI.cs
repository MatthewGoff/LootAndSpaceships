using System.Collections.Generic;
using UnityEngine;

public class DroneAI : AI
{
    private static readonly float ORBIT_DISTANCE = 5f;
    private static readonly float ORBIT_ANGULAR_SPEED = 60f;

    private float OrbitAngle;
    private bool Aggroed;
    private float AggroCountdown;
    private Spaceship Mother;
    private float AgroDistance;
    private float DeagroDistance;
    private float DeagroTime;

    public DroneAI(Spaceship spaceship, Autopilot autopilot, Spaceship mother, string[] parameters) : base(spaceship, autopilot)
    {
        Mother = mother;
        spaceship.AttackType = Helpers.ParseAttackType(parameters[0]);
        spaceship.AttackMode = Helpers.ParseAttackMode(parameters[1]);
        AgroDistance = Helpers.ParseFloat(parameters[2]);
        DeagroDistance = Helpers.ParseFloat(parameters[3]);
        DeagroTime = Helpers.ParseFloat(parameters[4]);

        OrbitAngle = 0f;
    }

    public override void Update(Dictionary<int, RadarProfile> radarProfiles)
    {
        CheckReload();

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

        if (closestEnemyDistance < AgroDistance)
        {
            Aggroed = true;
            AggroCountdown = DeagroTime;
        }
        else if (Aggroed == true && closestEnemyDistance > DeagroDistance)
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
            Spaceship.QueueAttack();
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
        AggroCountdown = DeagroTime;
    }
}
