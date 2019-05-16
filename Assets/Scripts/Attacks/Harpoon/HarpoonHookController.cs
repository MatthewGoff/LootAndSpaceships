using System.Collections;
using UnityEngine;

public class HarpoonHookController : MonoBehaviour
{
    private HarpoonAttackManager Manager;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (Manager.State == HarpoonState.Fireing && collider.tag == "Hitbox")
        {
            Combatant other = collider.gameObject.GetComponent<Combatant>();
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
