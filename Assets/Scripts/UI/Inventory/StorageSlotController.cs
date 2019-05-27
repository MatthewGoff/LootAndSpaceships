using UnityEngine;

public class StorageSlotController : MonoBehaviour, IAcceptsItems
{
    public GameObject ItemIconPrefab;

    private InventoryGUIController InventoryGUIController;
    private Item Item;
    private GameObject ItemIcon;
    private InventoryAddress InventoryAddress;

    public void Initialize(InventoryGUIController inventoryGUIController, InventoryAddress inventoryAddress, Item item)
    {
        InventoryGUIController = inventoryGUIController;
        Item = item;
        InventoryAddress = inventoryAddress;

        if (Item != null)
        {
            ItemIcon = Instantiate(ItemIconPrefab, transform);
            ItemIcon.GetComponent<ItemIconController>().Initialize(ItemSprites.Instance.GetItemSprites(Item.ItemType), Item.Colors);
        }
    }

    public void ClickEvent()
    {
        if (Item == null)
        {
            InventoryGUIController.RequestCursorItem(InventoryAddress);
        }
        else
        {
            Item previousItem = Item;
            bool itemPickedup = InventoryGUIController.PickupItem(Item, InventoryAddress);
            // If (our item was taken) and (our item wasn't replaced with a new item)
            // Then: This slot is now empty.
            if (itemPickedup && Item == previousItem)
            {
                Item = null;
                Destroy(ItemIcon);
            }
        }
    }

    public void TakeItem(Item item)
    {
        Clear();

        Item = item;
        ItemIcon = Instantiate(ItemIconPrefab, transform);
        ItemIcon.GetComponent<ItemIconController>().Initialize(ItemSprites.Instance.GetItemSprites(Item.ItemType), Item.Colors);
    }

    private void Clear()
    {
        Item = null;
        Destroy(ItemIcon);
    }
}
