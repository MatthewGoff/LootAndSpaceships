using UnityEngine;

public class SimpleAI : AI
{
    private static readonly float AGRO_DISTANCE = 20f;
    private static readonly float DEAGRO_DISTANCE = 30f;
    private static readonly float DEAGRO_TIME = 3f;

    private Vector2 Home;

    private bool Aggroed;
    private ITargetable AggroHolder;

    public SimpleAI(Spaceship spaceship, Autopilot autopilot) : base(spaceship, autopilot)
    { 
        Home = spaceship.GetPosition();
    }
	
	public override void Update ()
    {
        CheckAggro();


	}

    private void CheckAggro() 
    {
        Vector2 playerPosition = Vector2.zero;
        float distance = (Spaceship.GetPosition() - playerPosition).magnitude;
    }

    public override void AlertDamage(Combatant attacker)
    {
        
    }
}
