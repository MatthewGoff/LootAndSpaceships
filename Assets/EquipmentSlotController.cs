using UnityEngine;

public class EquipmentSlotController : MonoBehaviour
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

    public void Refresh(Item item)
    {
        if (item == null)
        {
            EmptyIcon.SetActive(true);
        }
        else
        {
            Item = item;
            ItemIcon = Instantiate(ItemIconPrefab, transform);
            ItemIcon.GetComponent<ItemIconController>().Initialize(ItemSprites.Instance.HullSprites, item.Colors);
            EmptyIcon.SetActive(false);
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
            EmptyIcon.SetActive(true);
        }
    }
}
