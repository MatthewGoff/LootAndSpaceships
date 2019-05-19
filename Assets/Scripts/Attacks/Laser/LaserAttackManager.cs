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

    public LaserAttackManager(Spaceship attacker, float damage)
    {
        Attacker = attacker;
        Damage = damage;
        ImmunityDuration = IMMUNITY_DURATION;

        GameObject gameObject = GameManager.Instance.Instantiate(Prefabs.Instance.LaserBeam, Vector2.zero, Quaternion.identity);
        gameObject.transform.SetParent(attacker.transform);
        gameObject.transform.localPosition = Vector2.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = new Vector2(BEAM_LENGTH, BEAM_WIDTH);
        Laser = gameObject.GetComponent<LaserController>();
        Laser.Initialize(this);
        TurnOff();
    }

    public void TurnOn(Vector2 attackVector, bool hasTarget, int targetUID)
    {
        Laser.TurnOn(attackVector, hasTarget, targetUID);
    }

    public void TurnOff()
    {
        Laser.TurnOff();
    }

    public void ResolveCollision(Spaceship other)
    {
        float damage = Random.Range(Damage * 0.5f, Damage * 1.5f);
        other.TakeDamage(this, damage, DamageType.Laser);
    }
}
