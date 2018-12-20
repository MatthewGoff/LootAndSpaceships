using UnityEngine;
using UnityEngine.UI;

public class FDNCanvasController : MonoBehaviour
{
    private static readonly Vector2 HIGHLIGHT_OFFSET = new Vector2(-1f / 54f, 1f / 54f);

    public AnimationCurve TextSize = new AnimationCurve(
        new Keyframe(0f, 14f),
        new Keyframe(1f, 60f)
    );
    public Gradient TextColor = new Gradient();

    public GameObject Subject;
    public float VerticalOffset;

    public void Display(int damage, float intensity)
    {
        // Highlight
        GameObject fdn = Instantiate(Prefabs.FDN, Vector2.zero, Quaternion.identity);
        fdn.transform.SetParent(transform);
        fdn.transform.localPosition = HIGHLIGHT_OFFSET;
        Text fdnText = fdn.GetComponent<Text>();
        fdnText.text = damage.ToString();
        fdnText.color = Color.white;
        fdnText.fontSize = Mathf.RoundToInt(TextSize.Evaluate(intensity));

        // Shadow
        fdn = Instantiate(Prefabs.FDN, Vector2.zero, Quaternion.identity);
        fdn.transform.SetParent(transform);
        fdn.transform.localPosition = -HIGHLIGHT_OFFSET;
        fdnText = fdn.GetComponent<Text>();
        fdnText.text = damage.ToString();
        fdnText.color = Color.black;
        fdnText.fontSize = Mathf.RoundToInt(TextSize.Evaluate(intensity));

        // Text
        fdn = Instantiate(Prefabs.FDN, Vector2.zero, Quaternion.identity);
        fdn.transform.SetParent(transform);
        fdn.transform.localPosition = Vector2.zero;
        fdnText = fdn.GetComponent<Text>();
        fdnText.text = damage.ToString();
        fdnText.color = TextColor.Evaluate(intensity);
        fdnText.fontSize = Mathf.RoundToInt(TextSize.Evaluate(intensity));
    }

    private void LateUpdate()
    {
        transform.position = Subject.transform.position + new Vector3(0f, VerticalOffset);
    }
}
