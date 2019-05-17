using System.Collections;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private float Duration = 1f;
    public AnimationCurve AlphaCurve = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(1f, 0f)
    );
    private RocketAttackManager Manager;
    private SpriteRenderer SpriteRenderer;

    private void Awake ()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("Explode");
    }

    private IEnumerator Explode()
    {
        for (float i = 0f; i < Duration; i += Time.deltaTime)
        {
            if (i == 1)
            {
                Destroy(GetComponent<CircleCollider2D>());
            }
            Color color = SpriteRenderer.color;
            color.a = AlphaCurve.Evaluate(i / Duration);
            SpriteRenderer.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            Spaceship other = collider.gameObject.GetComponent<Spaceship>();
            if (other.Team != Manager.Attacker.Team)
            {
                Manager.ResolveCollision(other);
            }
        }
    }

    public void AssignManager(RocketAttackManager manager)
    {
        Manager = manager;
    }
}
