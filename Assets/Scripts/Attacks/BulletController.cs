using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float Duration = 3f; // In seconds

    private BulletAttackManager Manager;
    
    private void Awake()
    {
        StartCoroutine("ExpirationCountdown");
    }

    private IEnumerator ExpirationCountdown()
    {
        yield return new WaitForSeconds(Duration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            Combatant other = collider.gameObject.GetComponent<Combatant>();
            if (other.Team != Manager.Originator.Team)
            {
                Destroy(gameObject);
                Manager.ResolveCollision(other);
            }
        }
    }

    public void AssignManager(BulletAttackManager manager)
    {
        Manager = manager;
    }
}
