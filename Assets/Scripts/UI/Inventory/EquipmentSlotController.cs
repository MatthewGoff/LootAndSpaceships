using UnityEngine;

public class EquipmentSlotController : MonoBehaviour, IAcceptsItems
{
    public int EquipmentIndex;
    public GameObject EmptyIcon;
    public InventoryGUIController InventoryGUIController;
    public GameObject ItemIconPrefab;

    private Item Item;
    private GameObject ItemIcon;
    private InventoryAddress InventoryAddress;

    private void Awake()
    {
        InventoryAddress = InventoryAddress.NewEquipmentAddress(EquipmentIndex);
    }

    public void Initialize(Item item)
    {
        if (item != null)
        {
            TakeItem(item);
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
                EmptyIcon.SetActive(true);
            }
        }
    }

    public void TakeItem(Item item)
    {
        Clear();

        Item = item;
        ItemIcon = Instantiate(ItemIconPrefab, transform);
        ItemIcon.GetComponent<ItemIconController>().Initialize(ItemSprites.Instance.GetItemSprites(Item.ItemType), Item.Colors);
        EmptyIcon.SetActive(false);
    }

    private void Clear()
    {
        Item = null;
        Destroy(ItemIcon);
        EmptyIcon.SetActive(true);
    }
}
