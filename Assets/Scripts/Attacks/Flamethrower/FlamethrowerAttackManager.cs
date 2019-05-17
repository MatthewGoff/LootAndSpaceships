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

        Quaternion quaternion = Quaternion.Euler(0, 0, attacker.GetComponent<Rigidbody2D>().rotation);
        GameObject gameObject = GameObject.Instantiate(Prefabs.Instance.Flamethrower, attacker.Position, quaternion);
        gameObject.transform.SetParent(attacker.transform);
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

    public void TurnOn()
    {
        Flamethrower.TurnOn();
    }

    public void TurnOff()
    {
        Flamethrower.TurnOff();
    }
}
