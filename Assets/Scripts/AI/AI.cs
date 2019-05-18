using System.Collections.Generic;

public abstract class AI
{
    protected readonly Spaceship Spaceship;
    protected readonly Autopilot Autopilot;

    public AI(Spaceship spaceship, Autopilot autopilot)
    {
        Spaceship = spaceship;
        Autopilot = autopilot;
    }

    public abstract void Update(Dictionary<int, RadarProfile> radarProfiles);
    public abstract void AlertDamage(Spaceship attacker);

    public static AI CreateAI(AIType aiType, Spaceship spaceship, Autopilot autopilot, AttackType attackTypes)
    {
        if (aiType == AIType.Player)
        {
            return null;
        }
        else if (aiType == AIType.PassiveAI)
        {
            return new PassiveAI(spaceship, autopilot);
        }
        else if (aiType == AIType.SimpleAI)
        {
            return new SimpleAI(spaceship, autopilot, attackTypes);
        }
        else
        {
            return null;
        }
    }
}
