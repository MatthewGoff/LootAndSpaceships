using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private static readonly int STORAGE_PAGES = 4;
    private static readonly int STORAGE_ROWS = 20;
    public static readonly int EQUIPMENT_SLOTS = 20;

    public List<Item> Inbox { get; private set; }
    public Item[,,] Storage { get; private set; }
    public Item[] Equipment { get; private set; }

    public Inventory()
    {
        Inbox = new List<Item>();
        Storage = new Item[STORAGE_PAGES, 5, STORAGE_ROWS];
        Equipment = new Item[EQUIPMENT_SLOTS];
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

    public void MoveItem(Item item, InventoryAddress address)
    {
        RemoveItemFromInventory(item);
        if (address.InventorySection == InventorySection.Storage)
        {
            Storage[address.StoragePage, address.StorageColumn, address.StorageRow] = item;
        }
        else if (address.InventorySection == InventorySection.Equipment)
        {
            Equipment[address.EquipmentIndex] = item;
        }
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
                        return;
                    }
                }
            }
        }
        for (int i = 0; i < Equipment.Length; i++)
        {
            if (Equipment[i] == item)
            {
                Equipment[i] = null;
                return;
            }
        }
    }

    public void Scrap(Item item)
    {
        Inbox.Remove(item);
    }
}
