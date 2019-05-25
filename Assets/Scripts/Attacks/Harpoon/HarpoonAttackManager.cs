using UnityEngine;

public class HarpoonAttackManager : AttackManager
{
    public HarpoonState State;

    private GameObject Harpoon;
    private readonly float Damage;

    public HarpoonAttackManager(Spaceship attacker, Vector2 position, Vector2 direction, Vector2 initialVelocity, float damage, float speed, float range, float duration)
    {
        Attacker = attacker;
        Damage = damage;
        State = HarpoonState.Fireing;

        Harpoon = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.Harpoon, Vector2.zero, Quaternion.identity);
        Harpoon.GetComponent<HarpoonController>().Initialize(this, attacker, position, direction, initialVelocity, speed, range, duration);
    }

    public void LockHarpoon(Spaceship other)
    {
        State = HarpoonState.Locked;
        other.TakeDamage(this, Damage, DamageType.Physical);
        Harpoon.GetComponent<HarpoonController>().Lock(other);
    }

    public void Retrieve()
    {
        if (State == HarpoonState.Locked)
        {
            Harpoon.GetComponent<HarpoonController>().Unlock();
        }
        State = HarpoonState.Retrieving;
    }

    public void Expire()
    {
        State = HarpoonState.Expired;
    }
}
