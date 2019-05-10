﻿using UnityEngine;

public class RocketAttackManager : AttackManager
{
    public static readonly float Recoil = 1f;

    public bool HasTarget;
    public int TargetUID;

    private readonly float AOE = 2f;
    private readonly float InitialSpeed = 1f; // In units per second
    private readonly int Damage;

    public RocketAttackManager(Combatant attacker, bool hasTarget, int targetUID, Vector2 position, Vector2 direction, Vector2 initialVelocity, int damage)
    {
        Attacker = attacker;
        HasTarget = hasTarget;
        TargetUID = targetUID;
        Damage = damage;

        float angle = Vector2.SignedAngle(Vector2.right, direction);
        GameObject rocket = GameObject.Instantiate(Prefabs.Instance.Rocket, position, Quaternion.Euler(0, 0, angle));
        rocket.GetComponent<RocketController>().AssignManager(this);
        rocket.GetComponent<Rigidbody2D>().velocity = InitialSpeed * direction.normalized + initialVelocity;
    }

    public void Explode(Vector2 position)
    {
        GameObject explosion = GameObject.Instantiate(Prefabs.Instance.Explosion, position, Quaternion.identity);
        explosion.GetComponent<ExplosionController>().AssignManager(this);
        explosion.transform.localScale = new Vector2(AOE, AOE);
    }

    public void ResolveCollision(Combatant other)
    {
        other.TakeDamage(Attacker, Damage, DamageType.Explosion);
    }
}
