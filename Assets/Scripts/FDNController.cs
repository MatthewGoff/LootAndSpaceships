using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FDNController : MonoBehaviour
{
    public static readonly float VERTICAL_FDN_OFFSET = 0.4f;
    private static readonly Vector2 HIGHLIGHT_OFFSET = new Vector2(-1f / 54f, 1f / 54f);

    public AnimationCurve TextSize = new AnimationCurve(
        new Keyframe(0f, 14f),
        new Keyframe(1f, 60f)
    );
    public Gradient TextColor = new Gradient();
    public AnimationCurve AlphaCurve = new AnimationCurve(
        new Keyframe(0.0f, 1.0f),
        new Keyframe(0.5f, 1.0f),
        new Keyframe(1.0f, 0.0f)
    );

    public float Speed = 2f;
    public float Duration = 2f;

    private GameObject Text;
    private GameObject TextHighlight;
    private GameObject TextShadow;

    public void Display(int damage, float intensity)
    {
        transform.position = transform.position + new Vector3(0, VERTICAL_FDN_OFFSET, 0);

        // Highlight
        TextHighlight = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.FDNText, Vector2.zero, Quaternion.identity, transform);
        TextHighlight.transform.localPosition = HIGHLIGHT_OFFSET;
        Text text = TextHighlight.GetComponent<Text>();
        text.text = damage.ToString();
        text.color = Color.white;
        text.fontSize = Mathf.RoundToInt(TextSize.Evaluate(intensity));

        // Shadow
        TextShadow = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.FDNText, Vector2.zero, Quaternion.identity, transform);
        TextShadow.transform.localPosition = -HIGHLIGHT_OFFSET;
        text = TextShadow.GetComponent<Text>();
        text.text = damage.ToString();
        text.color = Color.black;
        text.fontSize = Mathf.RoundToInt(TextSize.Evaluate(intensity));

        // Text
        Text = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.FDNText, Vector2.zero, Quaternion.identity, transform);
        Text.transform.localPosition = Vector2.zero;
        text = Text.GetComponent<Text>();
        text.text = damage.ToString();
        text.color = TextColor.Evaluate(intensity);
        text.fontSize = Mathf.RoundToInt(TextSize.Evaluate(intensity));

        StartCoroutine("Float");
    }


    private IEnumerator Float()
    {
        for (float t = 0f; t < Duration; t += Time.deltaTime)
        {
            Text.transform.localPosition += new Vector3(0f, Speed * Time.deltaTime);
            TextShadow.transform.localPosition += new Vector3(0f, Speed * Time.deltaTime);
            TextHighlight.transform.localPosition += new Vector3(0f, Speed * Time.deltaTime);

            float alpha = AlphaCurve.Evaluate(t / Duration);
            Color color = Text.GetComponent<Text>().color;
            color.a = alpha;
            Text.GetComponent<Text>().color = color;
            color = TextShadow.GetComponent<Text>().color;
            color.a = alpha;
            TextShadow.GetComponent<Text>().color = color;
            color = TextHighlight.GetComponent<Text>().color;
            color.a = alpha;
            TextHighlight.GetComponent<Text>().color = color;

            yield return null;
        }
        Destroy(gameObject);
    }
}
