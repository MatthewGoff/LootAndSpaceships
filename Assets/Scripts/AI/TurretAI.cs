using System;
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

    public TurretAI(Spaceship spaceship, Autopilot autopilot, string[] parameters) : base(spaceship, autopilot)
    {
        spaceship.AttackType = Helpers.ParseAttackType(parameters[0]);
        spaceship.AttackMode = Helpers.ParseAttackMode(parameters[1]);
        AgroDistance = Helpers.ParseFloat(parameters[2]);
        DeagroDistance = Helpers.ParseFloat(parameters[3]);
        DeagroTime = Helpers.ParseFloat(parameters[4]);
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
            Spaceship.SelectTarget(closestEnemyUID);
            Spaceship.QueueAttack();
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
