using System.Collections;
using UnityEngine;

public class HarpoonHookController : MonoBehaviour
{
    private HarpoonAttackManager Manager;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (Manager.State == HarpoonState.Fireing && collider.tag == "Hitbox")
        {
            Spaceship other = collider.gameObject.GetComponent<Spaceship>();
            if (other.Team != Manager.Attacker.Team)
            {
                Manager.LockHarpoon(other);
            }
        }
    }
    public void AssignManager(HarpoonAttackManager manager)
    {
        Manager = manager;
    }
}
