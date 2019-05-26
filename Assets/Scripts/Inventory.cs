using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private static readonly int INVENTORY_PAGES = 4;
    private static readonly int INVENTORY_ROWS = 20;
    public List<Item> Inbox { get; private set; }
    public Item[,,] Storage { get; private set; }

    public Inventory()
    {
        Inbox = new List<Item>();
        Storage = new Item[INVENTORY_PAGES, 5, INVENTORY_ROWS];
    }

    public void Pickup(Item item)
    {
        Inbox.Add(item);
    }

    public void Keep(Item item)
    {
        for (int page = 0; page < Storage.GetLength(0); page++)
        {
            for (int row = 0; row < Storage.GetLength(2); row++)
            {
                for (int col = 0; col < Storage.GetLength(1); col++)
                {
                    if (Storage[page, col, row] == null)
                    {
                        Storage[page, col, row] = item;
                        Inbox.Remove(item);
                        return;
                    }
                }
            }
        }
    }

    public void MoveItemToStorage(Item item, int destinationPage, int destinationCol, int destinationRow)
    {
        RemoveItemFromInventory(item);
        Storage[destinationPage, destinationCol, destinationRow] = item;
    }

    public void RemoveItemFromInventory(Item item)
    {
        for (int page = 0; page < Storage.GetLength(0); page++)
        {
            for (int col = 0; col < Storage.GetLength(1); col++)
            {
                for (int row = 0; row < Storage.GetLength(2); row++)
                {
                    if (Storage[page, col, row] == item)
                    {
                        Storage[page, col, row] = null;
                    }
                }
            }
        }
    }

    public void Scrap(Item item)
    {
        Inbox.Remove(item);
    }
}
