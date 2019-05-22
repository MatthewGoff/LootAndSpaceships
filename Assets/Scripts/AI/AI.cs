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

    public static AI CreateAI(AIType aiType, Spaceship spaceship, Autopilot autopilot, Spaceship mother, string[] parameters)
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
            return new SimpleAI(spaceship, autopilot, parameters);
        }
        else if (aiType == AIType.DroneAI)
        {
            return new DroneAI(spaceship, autopilot, mother, parameters);
        }
        else if (aiType == AIType.TurretAI)
        {
            return new TurretAI(spaceship, autopilot, parameters);
        }
        else
        {
            return null;
        }
    }
}
