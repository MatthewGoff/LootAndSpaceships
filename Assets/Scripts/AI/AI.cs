public abstract class AI
{
    private readonly Spaceship Spaceship;
    private readonly Autopilot Autopilot;

    public AI(Spaceship spaceship, Autopilot autopilot)
    {
        Spaceship = spaceship;
        Autopilot = autopilot;
    }

    public abstract void Update();
    public abstract void TakeDamage(Combatant attacker);
}
