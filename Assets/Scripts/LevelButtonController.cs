using UnityEngine;
using UnityEngine.UI;

public class LevelButtonController : MonoBehaviour
{
    public GameObject Button;
    public GameObject Lock;

    private void Start()
    {
        Button.GetComponent<Button>().interactable = false;
        Lock.SetActive(true);
    }

    public void Unlock()
    {
        Button.GetComponent<Button>().interactable = true;
        Lock.SetActive(false);
    }
}
