using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpBlastController : MonoBehaviour
{
    public AnimationCurve RadiusCurve;
    public AnimationCurve AlphaCurve;
    public float BlastDuration;

    private float Radius;
    private float Alpha;

    private void Start()
    {
        StartCoroutine("Blast");
    }

    public IEnumerator Blast()
    {
        Material material = GetComponent<SpriteRenderer>().material;
        for (float t = 0; t < BlastDuration; t += Time.deltaTime)
        {
            Radius = (transform.localScale.x / 2) * RadiusCurve.Evaluate(t / BlastDuration);
            material.SetFloat("radius", Radius);
            Alpha = AlphaCurve.Evaluate(t / BlastDuration);
            material.SetFloat("masterAlpha", Alpha);
            if (GameManager.Instance.PlayerAlive)
            {
                Vector2 playerPosition = GameManager.Instance.PlayerController.Position;
                transform.position = playerPosition;
                material.SetVector("position", playerPosition);
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
