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
