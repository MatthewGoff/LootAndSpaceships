using System.Collections.Generic;
using UnityEngine;

public class PassiveAI : AI
{
    public PassiveAI(Spaceship spaceship, Autopilot autopilot) : base(spaceship, autopilot)
    {
        autopilot.Standby();
    }

    public override void Update(Dictionary<int, RadarProfile> radarProfiles)
    {
    }

    public override void AlertDamage(Spaceship attacker)
    {
    }
}
