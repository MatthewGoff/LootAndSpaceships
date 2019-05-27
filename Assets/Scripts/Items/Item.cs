using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    protected float Weight;
    protected float Volume;
    public ItemClass ItemClass;
    public ItemType ItemType;
    public Color[] Colors;

    protected Item(float weight, float volume, ItemClass itemClass, ItemType itemType)
    {
        Weight = weight;
        Volume = volume;
        ItemClass = itemClass;
        ItemType = itemType;

        ChooseRandomColors2();
    }

    private void ChooseRandomColors()
    {
        int numberOfColors = 4;
        float minimumHueDistance = 0.15f;
        Colors = new Color[numberOfColors];
        float hueOffset = Random.value;
        float[] hues = new float[numberOfColors];
        hues[0] = 0;
        for (int i = 1; i < hues.Length; i++)
        {
            hues[i] = Random.Range(hues[i-1] + minimumHueDistance,  i * (1 - minimumHueDistance) / (hues.Length - 1));
        }
        for (int i = 0; i < Colors.Length; i++)
        {
            Colors[i] = GetRandomItemColor((hueOffset+hues[i]) % 1);
        }
    }

    private static Color GetRandomItemColor(float hue)
    {
        float sample = Random.value;
        if (sample < 0.2)
        {
            float value = Mathf.RoundToInt(5 * hue) / 5f;
            return Color.HSVToRGB(0, 0, value);
        }
        else if (sample < 0.6)
        {
            return Color.HSVToRGB(hue, 1f, Random.Range(0.75f, 1f));
        }
        else
        {
            return Color.HSVToRGB(hue, Random.Range(0.75f, 1f), 1f);
        }
    }

    private void ChooseRandomColors2()
    {
        int numberOfColors = 4;
        Colors = new Color[numberOfColors];

        List<float> valueOptions = new List<float>() { 0.0f, 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
        float[] values = new float[numberOfColors];
        for (int i = 0; i < values.Length; i++)
        {
            int selection = Random.Range(0, valueOptions.Count);
            values[i] = valueOptions[selection];
            valueOptions.Remove(valueOptions[selection]);
        }

        int colorIndex = Random.Range(0, numberOfColors);
        for (int i = 0; i < Colors.Length; i++)
        {
            if (i == colorIndex)
            {
                if (Random.value < 0.5)
                {
                    Colors[i] = Color.HSVToRGB(Random.value, 1f, Random.Range(0.75f, 1f));
                }
                else
                {
                    Colors[i] = Color.HSVToRGB(Random.value, Random.Range(0.75f, 1f), 1f);
                }
            }
            else
            {
                Colors[i] = Color.HSVToRGB(0, 0, values[i]);
            }
        }
    }

    public static ItemType GetRandomItemType()
    {
        System.Array array = System.Enum.GetValues(typeof(ItemType));
        int selection = Mathf.Clamp(Mathf.FloorToInt(Random.Range(0, array.Length)), 0, array.Length - 1);
        return (ItemType)array.GetValue(selection);
    }

    public static Item CreateRandomItem(int level, ItemType itemType)
    {
        if (itemType == ItemType.Cannon)
        {
            return Cannon.CreateRandomCannon(level);
        }
        else if (itemType == ItemType.EMPGenerator)
        {
            return EMPGenerator.CreateRandomEMPGenerator(level);
        }
        else if (itemType == ItemType.Engine)
        {
            return Engine.CreateRandomEngine(level);
        }
        else if (itemType == ItemType.Flamethrower)
        {
            return Flamethrower.CreateRandomFlamethrower(level);
        }
        else if (itemType == ItemType.Hangar)
        {
            return Hangar.CreateRandomHangar(level);
        }
        else if (itemType == ItemType.Harpoon)
        {
            return Harpoon.CreateRandomHarpoon(level);
        }
        else if (itemType == ItemType.Hull)
        {
            return Hull.CreateRandomHull(level);
        }
        else if (itemType == ItemType.Laser)
        {
            return Laser.CreateRandomLaser(level);
        }
        else if (itemType == ItemType.LifeSupport)
        {
            return LifeSupport.CreateRandomLifeSupport(level);
        }
        else if (itemType == ItemType.MineLayer)
        {
            return MineLayer.CreateRandomMineLayer(level);
        }
        else if (itemType == ItemType.Reactor)
        {
            return Reactor.CreateRandomReactor(level);
        }
        else if (itemType == ItemType.RocketLauncher)
        {
            return RocketLauncher.CreateRandomRocketLauncher(level);
        }
        else if (itemType == ItemType.ShieldGenerator)
        {
            return ShieldGenerator.CreateRandomShieldGenerator(level);
        }
        else
        {
            return null;
        }
    }
}
