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

    public abstract void Update(LinkedList<RadarProfile> radarProfiles);
    public abstract void AlertDamage(Combatant attacker);
}
