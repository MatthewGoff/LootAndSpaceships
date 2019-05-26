using UnityEngine;

public class StorageSlotController : MonoBehaviour
{
    public GameObject ItemIconPrefab;

    private InventoryGUIController InventoryGUIController;
    private Item Item;
    private GameObject ItemIcon;
    private int Column;
    private int Row;

    public void Initialize(InventoryGUIController inventoryGUIController, int col, int row, Item item)
    {
        InventoryGUIController = inventoryGUIController;
        Item = item;
        Column = col;
        Row = row;

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
            InventoryGUIController.ItemRequestedByStorage(Column, Row);
        }
        else
        {
            bool itemTaken = InventoryGUIController.ItemSelected(Item, ItemIcon);
            if (itemTaken)
            {
                Item = null;
            }
        }
    }
}
