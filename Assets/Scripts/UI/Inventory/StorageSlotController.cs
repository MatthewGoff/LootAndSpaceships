using UnityEngine;

public class StorageSlotController : MonoBehaviour
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
            ItemIcon.GetComponent<ItemIconController>().Initialize(ItemSprites.Instance.HullSprites, item.Colors);
        }
    }

    public void ClickEvent()
    {
        if (Item == null)
        {
            InventoryGUIController.ItemRequested(InventoryAddress);
        }
        else
        {
            InventoryGUIController.ItemSelected(Item, ItemIcon, InventoryAddress);
            Item = null;
        }
    }
}
