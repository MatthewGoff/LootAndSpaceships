using UnityEngine;

public class ItemSprites : MonoBehaviour
{
    public static ItemSprites Instance;

    public Sprite[] CannonSprites;
    public Sprite[] EMPGeneratorSprites;
    public Sprite[] EngineSprites;
    public Sprite[] FlamethrowerSprites;
    public Sprite[] HangarSprites;
    public Sprite[] HarpoonSprites;
    public Sprite[] HullSprites;
    public Sprite[] LaserSprites;
    public Sprite[] LifeSupportSprites;
    public Sprite[] MineLayerSprites;
    public Sprite[] ReactorSprites;
    public Sprite[] RocketLauncherSprites;
    public Sprite[] ShieldSprites;

    private void Awake()
    {
        Instance = this;
    }

    public Sprite[] GetItemSprites(ItemType itemType)
    {
        if (itemType == ItemType.Engine)
        {
            return EngineSprites;
        }
        else if (itemType == ItemType.Hull)
        {
            return HullSprites;
        }
        else if (itemType == ItemType.LifeSupport)
        {
            return LifeSupportSprites;
        }
        else if (itemType == ItemType.Reactor)
        {
            return ReactorSprites;
        }
        else if (itemType == ItemType.ShieldGenerator)
        {
            return ShieldSprites;
        }
        else if (itemType == ItemType.Cannon)
        {
            return CannonSprites;
        }
        else if (itemType == ItemType.RocketLauncher)
        {
            return RocketLauncherSprites;
        }
        else if (itemType == ItemType.EMPGenerator)
        {
            return EMPGeneratorSprites;
        }
        else if (itemType == ItemType.Harpoon)
        {
            return HarpoonSprites;
        }
        else if (itemType == ItemType.Flamethrower)
        {
            return FlamethrowerSprites;
        }
        else if (itemType == ItemType.Laser)
        {
            return LaserSprites;
        }
        else if (itemType == ItemType.MineLayer)
        {
            return MineLayerSprites;
        }
        else if (itemType == ItemType.Hangar)
        {
            return HangarSprites;
        }
        else
        {
            return null;
        }
    }
}
