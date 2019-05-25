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

    public static AI CreateAI(AIParameters parameters, Spaceship spaceship, Autopilot autopilot, Spaceship mother)
    {
        if (parameters.AIType == AIType.Player)
        {
            return null;
        }
        else if (parameters.AIType == AIType.PassiveAI)
        {
            return new PassiveAI(spaceship, autopilot);
        }
        else if (parameters.AIType == AIType.SimpleAI)
        {
            return new SimpleAI(parameters, spaceship, autopilot);
        }
        else if (parameters.AIType == AIType.DroneAI)
        {
            return new DroneAI(parameters, spaceship, autopilot, mother);
        }
        else if (parameters.AIType == AIType.TurretAI)
        {
            return new TurretAI(parameters, spaceship, autopilot);
        }
        else
        {
            return null;
        }
    }
}
