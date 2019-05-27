using System.Collections.Generic;
using UnityEngine;

public class InventoryGUIController : MonoBehaviour
{
    public static InventoryGUIController Instance;

    public GameObject ItemIconPrefab;
    public GameObject InboxEntryPrefab;
    public GameObject StorageSlotPrefab;
    public GameObject InboxContent;
    public GameObject StorageContent;
    public GameObject InventoryGUI;
    public EquipmentSlotController[] EquipmentSlots;

    public bool ItemOnCursor
    {
        get
        {
            return CursorItem != null;
        }
    }

    private List<InboxEntryController> InboxEntries;
    private StorageSlotController[,] StorageSlots;
    private Item CursorItem;
    private GameObject CursorIcon;
    private int CurrentStoragePage;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InboxEntries = new List<InboxEntryController>();
        CurrentStoragePage = 0;
        InitializeInbox();
        InitializeEquipmentSlots();
        InitializeStorageSlots();
    }

    public void Open()
    {
        InventoryGUI.SetActive(true);
    }

    public void Close()
    {
        InventoryGUI.SetActive(false);
    }

    private void InitializeEquipmentSlots()
    {
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            EquipmentSlots[i].Initialize(GameManager.Instance.Player.Inventory.Equipment[i]);
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
        InboxContent.GetComponent<RectTransform>().sizeDelta = new Vector2(InboxEntryPrefab.GetComponent<RectTransform>().sizeDelta.x, 0);
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
        sizeDelta.y = GameManager.Instance.Player.Inventory.Storage.GetLength(2) * 74 - 10;
        StorageContent.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        StorageSlots = new StorageSlotController[GameManager.Instance.Player.Inventory.Storage.GetLength(1), GameManager.Instance.Player.Inventory.Storage.GetLength(2)];
        for (int col = 0; col < StorageSlots.GetLength(0); col++)
        {
            for (int row = 0; row < StorageSlots.GetLength(1); row++)
            {
                GameObject newStorageSlot = Instantiate(StorageSlotPrefab, StorageContent.transform);
                newStorageSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(col * 74, - row * 74);
                InventoryAddress address = InventoryAddress.NewStorageAddress(CurrentStoragePage, col, row);
                newStorageSlot.GetComponent<StorageSlotController>().Initialize(this, address, GameManager.Instance.Player.Inventory.Storage[CurrentStoragePage, col, row]);
                StorageSlots[col, row] = newStorageSlot.GetComponent<StorageSlotController>();
            }
        }
    }

    public void Keep(Item item, int inboxIndex)
    {
        InventoryAddress inventoryAddress = GameManager.Instance.Player.Inventory.Keep(item);
        if (inventoryAddress != null)
        {
            GetInventorySlot(inventoryAddress).TakeItem(item);
            RemoveFromInbox(inboxIndex);
        }
    }

    public void Scrap(Item item, int inboxIndex)
    {
        GameManager.Instance.Player.Inventory.Scrap(item);
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

    public void NewInboxItem()
    {
        GameObject newInboxEntry = Instantiate(InboxEntryPrefab, InboxContent.transform);

        int spaceing;
        if (GameManager.Instance.Player.Inventory.Inbox.Count == 1)
        {
            spaceing = 0;
        }
        else
        {
            spaceing = 10;
        }
        newInboxEntry.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, - InboxContent.GetComponent<RectTransform>().sizeDelta.y - spaceing);
        InboxContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 64 + spaceing);

        int index = GameManager.Instance.Player.Inventory.Inbox.Count - 1;
        newInboxEntry.GetComponent<InboxEntryController>().Initialize(this, GameManager.Instance.Player.Inventory.Inbox[index], index);
        InboxEntries.Add(newInboxEntry.GetComponent<InboxEntryController>());
    }

    /// <summary>
    /// Pickup an item from an inventory slot and put it on the cursor if possible.
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
                if (GameManager.Instance.Player.Inventory.GetClassRestrictions(inventoryAddress.EquipmentIndex).Contains(CursorItem.ItemClass))
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
        GameManager.Instance.Player.Inventory.MoveItem(CursorItem, inventoryAddress);
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
