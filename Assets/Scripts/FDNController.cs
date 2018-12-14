using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FDNController : MonoBehaviour {

    public AnimationCurve AlphaCurve = new AnimationCurve(
        new Keyframe(0.0f, 1.0f),
        new Keyframe(0.5f, 1.0f),
        new Keyframe(1.0f, 0.0f)
    );

    public float Speed = 1f;
    public float Distance = 5f;

    private Text Text;

    private void Awake()
    {
        Text = GetComponent<Text>();
        StartCoroutine("Float");
    }

    private IEnumerator Float()
    {
        for (transform.localPosition = Vector2.zero; transform.localPosition.y < Distance; transform.localPosition += new Vector3(0f, Speed * Time.deltaTime))
        {
            Color color = Text.color;
            color.a = AlphaCurve.Evaluate(transform.localPosition.y / Distance);
            Text.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }
}
