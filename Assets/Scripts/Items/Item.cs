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
            Colors[i] = Color.HSVToRGB(Random.value, Random.value, Random.value);
        }
    }
}
