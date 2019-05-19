using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionTypeAttackManager : AttackManager
{
    public abstract void ResolveCollision(Spaceship other);
}
