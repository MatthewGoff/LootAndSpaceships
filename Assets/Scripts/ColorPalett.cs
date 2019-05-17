using UnityEngine;

public class ColorPalett : MonoBehaviour
{
    public Color[] Colors;

    public Color GetColor(int i)
    {
        i--;
        if (i < Colors.Length)
        {
            return Colors[i];
        }
        else
        {
            return Color.white;
        }
    }
}
