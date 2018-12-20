using UnityEngine;

public class SimpleAI : AI
{
    private static readonly float AGRO_DISTANCE = 20f;
    private static readonly float DEAGRO_DISTANCE = 30f;
    private static readonly float DEAGRO_TIME = 3f;

    private Vector2 Home;

    private bool Agroed;
    private ITargetable Target;

    public SimpleAI(Spaceship spaceship, Autopilot autopilot) : base(spaceship, autopilot)
    {
        Home = spaceship.GetPosition();
    }
	
	public override void Update ()
    {
		
	}

    public override void TakeDamage(Combatant attacker)
    {
        
    }
}
