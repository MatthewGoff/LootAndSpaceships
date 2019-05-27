using System.Collections.Generic;
using UnityEngine;

public class InventoryGUIController : MonoBehaviour
{
    public GameObject ItemIconPrefab;
    public GameObject InboxEntryPrefab;
    public GameObject StorageSlotPrefab;
    public GameObject InboxContent;
    public GameObject StorageContent;
    public EquipmentSlotController[] EquipmentSlots;

    public bool ItemOnCursor
    {
        get
        {
            return CursorItem != null;
        }
    }
    private Inventory PlayerInventory;
    private List<InboxEntryController> InboxEntries;
    private StorageSlotController[,] StorageSlots;
    private Item CursorItem;
    private GameObject CursorIcon;
    private int CurrentStoragePage;

    private void Start()
    {
        InboxEntries = new List<InboxEntryController>();
        PlayerInventory = GameManager.Instance.Player.Inventory;
        CurrentStoragePage = 0;
        InitializeInbox();
        InitializeEquipmentSlots();
        InitializeStorageSlots();
    }

    private void InitializeEquipmentSlots()
    {
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            EquipmentSlots[i].Initialize(PlayerInventory.Equipment[i]);
        }
    }

    private void Update()
    {
        if (ItemOnCursor)
        {
            CursorIcon.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        }
    }

    private void InitializeInbox()
    {
        InboxContent.GetComponent<RectTransform>().sizeDelta = new Vector2(InboxEntryPrefab.GetComponent<RectTransform>().sizeDelta.x, PlayerInventory.Inbox.Count * 74 - 10);

        int verticalOffset = 0;
        for (int i = 0; i < PlayerInventory.Inbox.Count; i++)
        {
            GameObject newInboxEntry = Instantiate(InboxEntryPrefab, InboxContent.transform);
            newInboxEntry.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, verticalOffset);
            verticalOffset -= 74;
            newInboxEntry.GetComponent<InboxEntryController>().Initialize(this, PlayerInventory.Inbox[i], i);
            InboxEntries.Add(newInboxEntry.GetComponent<InboxEntryController>());
        }
    }

    private void ClearInbox()
    {
        if (InboxEntries != null)
        {
            foreach (InboxEntryController inboxEntry in InboxEntries)
            {
                Destroy(inboxEntry.gameObject);
            }
        }
        InboxEntries = new List<InboxEntryController>();
    }

    private void InitializeStorageSlots()
    {
        Vector2 sizeDelta = StorageContent.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = PlayerInventory.Storage.GetLength(2) * 74 - 10;
        StorageContent.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        StorageSlots = new StorageSlotController[PlayerInventory.Storage.GetLength(1), PlayerInventory.Storage.GetLength(2)];
        for (int col = 0; col < StorageSlots.GetLength(0); col++)
        {
            for (int row = 0; row < StorageSlots.GetLength(1); row++)
            {
                GameObject newStorageSlot = Instantiate(StorageSlotPrefab, StorageContent.transform);
                newStorageSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(col * 74, - row * 74);
                InventoryAddress address = InventoryAddress.NewStorageAddress(CurrentStoragePage, col, row);
                newStorageSlot.GetComponent<StorageSlotController>().Initialize(this, address, PlayerInventory.Storage[CurrentStoragePage, col, row]);
                StorageSlots[col, row] = newStorageSlot.GetComponent<StorageSlotController>();
            }
        }
    }

    public void Keep(Item item, int inboxIndex)
    {
        InventoryAddress inventoryAddress = PlayerInventory.Keep(item);
        if (inventoryAddress != null)
        {
            GetInventorySlot(inventoryAddress).TakeItem(item);
            RemoveFromInbox(inboxIndex);
        }
    }

    public void Scrap(Item item, int inboxIndex)
    {
        PlayerInventory.Scrap(item);
        RemoveFromInbox(inboxIndex);
    }

    private void RemoveFromInbox(int inboxIndex)
    {
        InboxEntryController temp = InboxEntries[inboxIndex];
        InboxEntries.Remove(temp);
        Destroy(temp.gameObject);

        for (int i = inboxIndex; i < InboxEntries.Count; i++)
        {
            InboxEntries[i].MoveUp();
        }

        InboxContent.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, 74);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="inventoryAddress"></param>
    /// <returns>
    /// true if the item was picked-up. false otherwise (i.e. There was an item
    /// already on the cursor which could not be put down)
    /// </returns>
    public bool PickupItem(Item item, InventoryAddress inventoryAddress)
    {
        RequestCursorItem(inventoryAddress);

        if (!ItemOnCursor)
        {
            CursorItem = item;
            CursorIcon = Instantiate(ItemIconPrefab, GameManager.Instance.RootCanvas.transform);
            CursorIcon.GetComponent<ItemIconController>().Initialize(ItemSprites.Instance.GetItemSprites(CursorItem.ItemType), CursorItem.Colors);
            CursorIcon.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Request that the item on the cursor be deposited at the specified
    /// inventory address.
    /// </summary>
    /// <param name="inventoryAddress"></param>
    public void RequestCursorItem(InventoryAddress inventoryAddress)
    {
        if (ItemOnCursor)
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
        GetInventorySlot(inventoryAddress).TakeItem(CursorItem);
        CursorItem = null;
        Destroy(CursorIcon);
    }

    private IAcceptsItems GetInventorySlot(InventoryAddress inventoryAddress)
    {
        if (inventoryAddress.InventorySection == InventorySection.Equipment)
        {
            return EquipmentSlots[inventoryAddress.EquipmentIndex];
        }
        else if (inventoryAddress.InventorySection == InventorySection.Storage)
        {
            if (inventoryAddress.StoragePage != CurrentStoragePage)
            {
                Debug.LogError("InventoryGUIController tried accessing a storage slot on an inactive page");
            }
            return StorageSlots[inventoryAddress.StorageColumn, inventoryAddress.StorageRow];
        }
        else
        {
            return null;
        }
    }
}
