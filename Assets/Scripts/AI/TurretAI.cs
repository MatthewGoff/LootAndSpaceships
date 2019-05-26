using System.Collections.Generic;
using UnityEngine;

public class TurretAI : AI
{
    private bool Aggroed;
    private float AggroCountdown;
    private AttackType AttackType;
    private float AgroDistance;
    private float DeagroDistance;
    private float DeagroTime;

    public TurretAI(AIParameters parameters, Spaceship spaceship, Autopilot autopilot) : base(spaceship, autopilot)
    {
        AgroDistance = Helpers.ParseFloat(parameters.Param1);
        DeagroDistance = Helpers.ParseFloat(parameters.Param2);
        DeagroTime = Helpers.ParseFloat(parameters.Param3);
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
            Spaceship.SelectTarget(closestEnemyUID);
            Spaceship.QueueAttack(1);
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
        AggroCountdown = DeagroTime;
    }
}
