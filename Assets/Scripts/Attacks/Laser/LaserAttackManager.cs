using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttackManager : AttackManager
{
    public readonly float IMMUNITY_DURATION = 0.1f;
    public readonly float BEAM_LENGTH = 10f;
    public readonly float BEAM_WIDTH = 0.5f;
    public readonly float ANGLE_LENIENCE = 10f;

    private float Damage;
    private LaserController Laser;

    public LaserAttackManager(Combatant attacker, float damage)
    {
        Attacker = attacker;
        Damage = damage;
        ImmunityDuration = IMMUNITY_DURATION;

        GameObject gameObject = GameObject.Instantiate(Prefabs.Instance.LaserBeam, Vector2.zero, Quaternion.identity);
        gameObject.transform.SetParent(attacker.gameObject.transform);
        gameObject.transform.localPosition = Vector2.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = new Vector2(BEAM_LENGTH, BEAM_WIDTH);
        Laser = gameObject.GetComponent<LaserController>();
        Laser.Initialize(this);
        TurnOff();
    }

    public void TurnOn(bool hasTarget, int targetUID, Vector2 headingVector)
    {
        float angle = 0f;
        if (hasTarget)
        {
            Vector2 position = RadarOmniscience.Instance.PingRadar()[targetUID].Position;
            Vector2 targetVector = position - (Vector2)Laser.gameObject.transform.position;
            angle = Vector2.SignedAngle(headingVector, targetVector);
            angle = Mathf.Clamp(angle, -ANGLE_LENIENCE, ANGLE_LENIENCE);
        }
        Laser.gameObject.SetActive(true);
        Laser.gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void TurnOff()
    {
        Laser.gameObject.SetActive(false);
    }

    public void ResolveCollision(Combatant other)
    {
        float damage = Random.Range(Damage * 0.5f, Damage * 1.5f);
        other.TakeDamage(this, damage, DamageType.Laser);
    }
}
