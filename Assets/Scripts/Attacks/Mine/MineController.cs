using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{
    private readonly float BLINK_PERIOD = 1f;
    private readonly float BLINK_DURATION = 0.2f;

    public GameObject SpriteColor1;
    public GameObject SpriteColor2;
    public GameObject SpriteColor3;

    private MineAttackManager Manager;

    public void Initialize(MineAttackManager manager)
    {
        Manager = manager;

        ColorPalett colorPalett = ConfigurationColorPaletts.Instance.GetColorPalett("Team " + Manager.Attacker.Team);
        SpriteColor1.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(2);
        SpriteColor2.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(1);

        StartCoroutine("Blink");
    }

    public IEnumerator Blink()
    {
        Color color = Color.red;
        for (float t = 0; true; t += Time.deltaTime)
        {
            float x = t % BLINK_PERIOD;
            if (x < BLINK_DURATION)
            {
                color.a = 1f;
            }
            else
            {
                color.a = 0f;
            }

            SpriteColor3.GetComponent<SpriteRenderer>().color = color;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Hitbox")
        {
            Spaceship spaceship = collider.GetComponent<Spaceship>();
            if (spaceship.Team != Manager.Attacker.Team)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        Manager.Explode(transform.position);
        Destroy(gameObject);
    }
}
