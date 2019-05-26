using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int Experience { get; private set; }
    public int Level { get; private set; }
    public Inventory Inventory { get; private set; }

    public Player()
    {
        Inventory = new Inventory();
        for (int i = 0; i < 100; i++)
        {
            ItemType itemType = Item.GetRandomItemType();
            Item item = Item.CreateRandomItem(0, itemType);
            Inventory.Pickup(item);
        }
        Experience = 0;
        Level = 0;
    }

    public void EarnExperience(int quantity)
    {
        Experience += quantity;
        if (Experience >= Configuration.ExpForLevel(Level + 1))
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        Experience = 0;
        GameManager.Instance.PlayerLevelUp(Level);
    }

    public void PickupItem(Item item)
    {
        Inventory.Pickup(item);
    }
}
