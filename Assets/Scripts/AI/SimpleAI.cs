using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : AI
{
    private static readonly float AGRO_DISTANCE = 10f;
    private static readonly float DEAGRO_DISTANCE = 20f;
    private static readonly float DEAGRO_TIME = 3f;

    private readonly Vector2 Home;

    private bool Aggroed;
    private float AggroCountdown;

    public SimpleAI(Spaceship spaceship, Autopilot autopilot) : base(spaceship, autopilot)
    { 
        Home = spaceship.Position;
    }
	
	public override void Update (Dictionary<int, RadarProfile> radarProfiles)
    {
        bool haveTarget = false;
        Vector2 closestEnemyPosition = Vector2.zero;
        float closestEnemyDistance = float.MaxValue;
        foreach (RadarProfile radarProfile in radarProfiles.Values)
        {
            if (radarProfile.Team != Spaceship.Team)
            {
                float distance = (Spaceship.Position - radarProfile.Position).magnitude;
                if (distance < closestEnemyDistance)
                {
                    haveTarget = true;
                    closestEnemyPosition = radarProfile.Position;
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

        if (Aggroed & haveTarget)
        {
            Autopilot.SetTarget(closestEnemyPosition, AutopilotBehaviour.Seek);
            Spaceship.FireBullet = true;
        }
        else
        {
            Autopilot.SetTarget(Home, AutopilotBehaviour.Arrive);
        }
    }

    public override void AlertDamage(Spaceship attacker)
    {
        Aggroed = true;
        AggroCountdown = DEAGRO_TIME;
    }
}
