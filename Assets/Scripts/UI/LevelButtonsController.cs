using System.Collections;
using UnityEngine;

public class LevelButtonsController : MonoBehaviour
{
    private readonly float FADE_IN_DURATION = 2f;
    public GameObject[] Buttons;
    private bool Hidden;

    private void Awake()
    {
        Hide();
    }

    public void UnlockButton(int number)
    {
        Buttons[number - 1].GetComponent<LevelButtonController>().Unlock();
    }

    public void Show()
    {
        Hidden = false;
        StartCoroutine("FadeIn");
        GetComponent<CanvasGroup>().interactable = true;
    }

    public IEnumerator FadeIn()
    {
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            yield return null;
        }
        for (float t = 0; t < FADE_IN_DURATION; t += Time.deltaTime)
        {
            if (!Hidden)
            {
                GetComponent<CanvasGroup>().alpha = (t / FADE_IN_DURATION);
            }
            yield return null;
        }
    }

    public void Hide()
    {
        Hidden = true;
        GetComponent<CanvasGroup>().alpha = 0f;
        GetComponent<CanvasGroup>().interactable = false;
    }
}
