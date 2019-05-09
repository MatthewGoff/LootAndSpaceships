using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : AI
{
    private static readonly float AGRO_DISTANCE = 15f;
    private static readonly float DEAGRO_DISTANCE = 30f;
    private static readonly float DEAGRO_TIME = 3f;

    private Vector2 Home;

    private bool Aggroed;

    public SimpleAI(Spaceship spaceship, Autopilot autopilot) : base(spaceship, autopilot)
    { 
        Home = spaceship.GetPosition();
    }
	
	public override void Update (LinkedList<RadarProfile> radarProfiles)
    {
        foreach (RadarProfile profile in radarProfiles)
        {
            if (profile.Team != Spaceship.Team)
            {
                Autopilot.SetTarget(profile.Position, AutopilotBehaviour.Seek);
                Spaceship.FireBullet = true;
            }
        }
	}

    private void CheckAggro(Vector2 playerPosition) 
    {
        float distance = (Spaceship.GetPosition() - playerPosition).magnitude;
        if (distance < AGRO_DISTANCE)
        {
            Aggroed = true;
        }
        if (Aggroed)
        {
            Autopilot.SetTarget(playerPosition, AutopilotBehaviour.Seek);
        }
    }

    public override void AlertDamage(Combatant attacker)
    {
        
    }
}
