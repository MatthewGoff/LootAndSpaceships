using UnityEngine;
using UnityEngine.UI;

public class LevelUpTextController : MonoBehaviour
{
    private readonly float DISPLAY_DURATION = 3f;
    private readonly float FADE_DURATION = 1f;

    public GameObject Text;
    public GameObject Highlight;
    public GameObject Shadow;

    private float RemainingDuration;

    public void Display(int level)
    {
        Text.GetComponent<Text>().text = "Level " + level;
        Highlight.GetComponent<Text>().text = "Level " + level;
        Shadow.GetComponent<Text>().text = "Level " + level;

        gameObject.SetActive(true);
        RemainingDuration = DISPLAY_DURATION;
    }

    void Update()
    {
        RemainingDuration -= Time.deltaTime;
        float alpha = RemainingDuration / FADE_DURATION;
        Color color = Text.GetComponent<Text>().color;
        color.a = alpha;
        Text.GetComponent<Text>().color = color;
        color = Highlight.GetComponent<Text>().color;
        color.a = alpha;
        Highlight.GetComponent<Text>().color = color;
        color = Shadow.GetComponent<Text>().color;
        color.a = alpha;
        Shadow.GetComponent<Text>().color = color;
        if ( RemainingDuration <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
