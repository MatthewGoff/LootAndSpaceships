using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerAttackManager : AttackManager
{
    private readonly float IMMUNITY_DURATION = 0.1f;
    private float Damage;
    private FlamethrowerController Flamethrower;

    public FlamethrowerAttackManager(Spaceship attacker, float damage)
    {
        Attacker = attacker;
        Damage = damage;
        ImmunityDuration = IMMUNITY_DURATION;


        GameObject gameObject = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.Flamethrower, Vector2.zero, Quaternion.identity, attacker.transform);
        gameObject.transform.localPosition = Vector2.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        Flamethrower = gameObject.GetComponent<FlamethrowerController>();
        Flamethrower.AssignManager(this);
    }

    public void ResolveCollision(Spaceship other)
    {
        float damage = Random.Range(Damage * 0.5f, Damage * 1.5f);
        other.TakeDamage(this, damage, DamageType.Fire);
    }

    public void TurnOn(Vector2 attackVector)
    {
        Flamethrower.TurnOn(attackVector);
    }

    public void TurnOff()
    {
        Flamethrower.TurnOff();
    }
}
