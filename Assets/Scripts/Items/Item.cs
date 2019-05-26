using UnityEngine;

public abstract class Item
{
    protected float Weight;
    protected float Volume;
    protected ItemClass ItemClass;
    public Color[] Colors;

    protected Item(float weight, float volume, ItemClass itemClass)
    {
        Weight = weight;
        Volume = volume;
        ItemClass = itemClass;

        ChooseColors();
    }

    protected void ChooseColors()
    {
        Colors = new Color[4];
        for (int i = 0; i < Colors.Length; i++)
        {
            Colors[i] = Color.HSVToRGB(Random.value, 0.8f, 0.8f);
        }
    }

    protected void ChooseColors2()
    {
        Colors = new Color[4];
        float hue = Random.value;
        Colors[0] = Color.HSVToRGB(hue, 0.8f, 0.8f);
        hue = (hue + 0.25f) % 1;
        Colors[1] = Color.HSVToRGB(hue, 0.8f, 0.8f);
        hue = (hue + 0.25f) % 1;
        Colors[2] = Color.HSVToRGB(hue, 0.8f, 0.8f);
        hue = (hue + 0.25f) % 1;
        Colors[3] = Color.HSVToRGB(hue, 0.8f, 0.8f);
    }
}
