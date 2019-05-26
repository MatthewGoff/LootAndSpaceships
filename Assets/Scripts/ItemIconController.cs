using UnityEngine;
using UnityEngine.UI;

public class ItemIconController : MonoBehaviour
{
    public GameObject[] IconLayers;

    public void Initialize(Sprite[] sprites, Color[] colors)
    {
        for (int i = 0; i < 4; i++)
        {
            IconLayers[i].GetComponent<Image>().sprite = sprites[i];
            IconLayers[i].GetComponent<Image>().color = colors[i];
        }
    }
}
