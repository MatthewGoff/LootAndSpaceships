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

        ChooseRandomColors();
    }

    protected void ChooseRandomColors()
    {
        Colors = new Color[4];
        for (int i = 0; i < Colors.Length; i++)
        {
            Colors[i] = Color.HSVToRGB(Random.value, Random.value, Random.value);
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
