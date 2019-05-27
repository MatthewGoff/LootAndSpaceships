using System.Collections.Generic;

public class Inventory
{
    private static readonly int STORAGE_PAGES = 4;
    private static readonly int STORAGE_ROWS = 20;
    public static readonly int EQUIPMENT_SLOTS = 20;

    private static readonly int HULL_SLOT = 0;
    private static readonly int REACTOR_SLOT = 1;
    private static readonly int ENGINE_SLOT = 2;
    private static readonly int SHIELD_SLOT = 3;
    private static readonly int LIFE_SUPPORT_SLOT = 4;
    private static readonly int[] WILDCARD_SLOTS = new int[] { 5, 6, 7, 8, 9 };
    private static readonly int[] WEAPON_SLOTS = new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns>
    /// The address of the inventory slot the item has be placed in. Returns
    /// null if the item was not placed (i.e. inventory is full)
    /// </returns>
    public InventoryAddress Keep(Item item)
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
                        return InventoryAddress.NewStorageAddress(page, col, row);
                    }
                }
            }
        }
        return null;
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

    public List<ItemClass> GetClassRestrictions(int equipmentSlot)
    {
        if (equipmentSlot == HULL_SLOT)
        {
            return new List<ItemClass>() { ItemClass.Hull };
        }
        else if (equipmentSlot == REACTOR_SLOT)
        {
            return new List<ItemClass>() { ItemClass.Reactor };
        }
        else if (equipmentSlot == ENGINE_SLOT)
        {
            return new List<ItemClass>() { ItemClass.Engine };
        }
        else if (equipmentSlot == SHIELD_SLOT)
        {
            return new List<ItemClass>() { ItemClass.ShieldGenerator };
        }
        else if (equipmentSlot == LIFE_SUPPORT_SLOT)
        {
            return new List<ItemClass>() { ItemClass.LifeSupport };
        }
        for (int i = 0; i < WILDCARD_SLOTS.Length; i++)
        {
            if (equipmentSlot == WILDCARD_SLOTS[i])
            {
                return new List<ItemClass>();
                /*
                return new List<ItemClass>()
                {
                    ItemClass.Hull,
                    ItemClass.Reactor,
                    ItemClass.Engine,
                    ItemClass.ShieldGenerator,
                    ItemClass.LifeSupport
                };
                */
            }
        }
        for (int i = 0; i < WEAPON_SLOTS.Length; i++)
        {
            if (equipmentSlot == WEAPON_SLOTS[i])
            {
                return new List<ItemClass>() { ItemClass.Weapon };
            }
        }
        return new List<ItemClass>();
    }
}
