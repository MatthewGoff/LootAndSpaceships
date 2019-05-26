using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InboxItemController : MonoBehaviour
{
    public GameObject ItemIconPrefab;
    private Item Item;
    private InventoryGUIController InventoryGUIController;

    public void Initialize(InventoryGUIController inventoryGUIController, Item item)
    {
        Item = item;
        InventoryGUIController = inventoryGUIController;

        GameObject gameObject = Instantiate(ItemIconPrefab, transform);
        gameObject.GetComponent<ItemIconController>().Initialize(ItemSprites.Instance.GetItemSprites(Item.ItemType), Item.Colors);
    }

    public void Keep()
    {
        InventoryGUIController.Keep(Item);
    }

    public void Scrap()
    {
        InventoryGUIController.Scrap(Item);
    }
}
