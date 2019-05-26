using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGUIController : MonoBehaviour
{
    public GameObject InboxItemPrefab;
    public GameObject StorageSlotPrefab;
    public GameObject InboxContent;
    public GameObject StorageContent;
    public EquipmentSlotController[] EquipmentSlots;

    private Inventory PlayerInventory;
    private List<GameObject> InboxItems;
    private GameObject[,] StorageSlots;
    private int InboxCount;
    private Item CursorItem;
    private GameObject CursorIcon;
    private int CurrentStoragePage;

    private void Start()
    {
        InboxItems = new List<GameObject>();
        PlayerInventory = GameManager.Instance.Player.Inventory;
        InboxCount = 0;
        CurrentStoragePage = 0;
        RefreshInbox();
        RefreshStorage();
        RefreshEquipment();
    }

    private void Update()
    {
        if (InboxCount != PlayerInventory.Inbox.Count)
        {
            RefreshInbox();
        }
        InboxCount = PlayerInventory.Inbox.Count;

        if (CursorItem != null)
        {
            CursorIcon.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        }
    }

    private void RefreshInbox()
    {
        ClearInbox();

        int verticalOffset = 0;
        foreach (Item item in PlayerInventory.Inbox)
        {
            GameObject newInboxItem = Instantiate(InboxItemPrefab, InboxContent.transform);
            newInboxItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, verticalOffset);
            verticalOffset -= 74;
            newInboxItem.GetComponent<InboxItemController>().Initialize(this, item);
            InboxItems.Add(newInboxItem);
        }

        InboxContent.GetComponent<RectTransform>().sizeDelta = new Vector2(InboxItemPrefab.GetComponent<RectTransform>().sizeDelta.x, -1 * verticalOffset);
    }

    private void ClearInbox()
    {
        if (InboxItems != null)
        {
            foreach (GameObject inboxItem in InboxItems)
            {
                Destroy(inboxItem);
            }
        }
        InboxItems = new List<GameObject>();
    }

    private void RefreshStorage()
    {
        ClearStorage();

        Vector2 sizeDelta = StorageContent.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = PlayerInventory.Storage.GetLength(2) * 74 - 10;
        StorageContent.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        for (int col = 0; col < StorageSlots.GetLength(0); col++)
        {
            for (int row = 0; row < StorageSlots.GetLength(1); row++)
            {
                GameObject newStorageSlot = Instantiate(StorageSlotPrefab, StorageContent.transform);
                newStorageSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(col * 74, - row * 74);
                InventoryAddress address = InventoryAddress.NewStorageAddress(CurrentStoragePage, col, row);
                newStorageSlot.GetComponent<StorageSlotController>().Initialize(this, address, PlayerInventory.Storage[CurrentStoragePage, col, row]);
                StorageSlots[col, row] = newStorageSlot;
            }
        }
    }

    private void ClearStorage()
    {
        if (StorageSlots != null)
        {
            for (int col = 0; col < StorageSlots.GetLength(0); col++)
            {
                for (int row = 0; row < StorageSlots.GetLength(1); row++)
                {
                    if (StorageSlots[col, row] != null)
                    {
                        Destroy(StorageSlots[col, row]);
                    }
                }
            }
        }

        StorageSlots = new GameObject[PlayerInventory.Storage.GetLength(1), PlayerInventory.Storage.GetLength(2)];
    }

    private void RefreshEquipment()
    {
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            EquipmentSlots[i].Refresh(PlayerInventory.Equipment[i]);
        }
    }

    public void Keep(Item item)
    {
        PlayerInventory.Keep(item);
        RefreshInbox();
        RefreshStorage();

    }

    public void Scrap(Item item)
    {
        PlayerInventory.Scrap(item);
        RefreshInbox();
    }

    public void ItemSelected(Item item, GameObject icon, InventoryAddress inventoryAddress)
    {
        if (CursorItem != null)
        {
            DepositCursorItem(inventoryAddress);
        }

        CursorItem = item;
        CursorIcon = icon;

        CursorIcon.transform.SetParent(GameManager.Instance.RootCanvas.transform);
        CursorIcon.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
    }

    public void ItemRequested(InventoryAddress inventoryAddress)
    {
        if (CursorItem != null)
        {
            if (inventoryAddress.InventorySection == InventorySection.Equipment)
            {
                if (PlayerInventory.GetClassRestrictions(inventoryAddress.EquipmentIndex).Contains(CursorItem.ItemClass))
                {
                    DepositCursorItem(inventoryAddress);
                }
            }
            else
            {
                DepositCursorItem(inventoryAddress);
            }
        }
    }

    public void DepositCursorItem(InventoryAddress inventoryAddress)
    {
        PlayerInventory.MoveItem(CursorItem, inventoryAddress);
        CursorItem = null;
        Destroy(CursorIcon);
        RefreshStorage();
        RefreshEquipment();
    }
}
