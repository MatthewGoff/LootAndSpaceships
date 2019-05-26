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

    public DroneAI(AIParameters parameters, Spaceship spaceship, Autopilot autopilot, Spaceship mother) : base(spaceship, autopilot)
    {
        Mother = mother;
        AgroDistance = Helpers.ParseFloat(parameters.Param1);
        DeagroDistance = Helpers.ParseFloat(parameters.Param2);
        DeagroTime = Helpers.ParseFloat(parameters.Param3);

        OrbitAngle = 0f;
    }

    public override void Update(Dictionary<int, RadarProfile> radarProfiles)
    {
        int closestEnemyUID = Omniscience.NULL_UID;
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

        if (Aggroed & closestEnemyUID != Omniscience.NULL_UID)
        {
            Autopilot.SetTarget(radarProfiles[closestEnemyUID].Position, AutopilotBehaviour.Seek);
            Spaceship.SelectTarget(closestEnemyUID);
            Spaceship.QueueAttack(1);
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
