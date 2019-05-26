using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGUIController : MonoBehaviour
{
    public GameObject InboxItemPrefab;
    public GameObject StorageSlotPrefab;
    public GameObject InboxContent;
    public GameObject StorageContent;

    private Inventory PlayerInventory;
    private List<GameObject> InboxItems;
    private int InboxCount;
    private Item CursorItem;
    private GameObject CursorIcon;

    private void Start()
    {
        InboxItems = new List<GameObject>();
        PlayerInventory = GameManager.Instance.Player.Inventory;
        InboxCount = 0;
        RefreshStorage();
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
        foreach (GameObject inboxItem in InboxItems)
        {
            Destroy(inboxItem);
        }
        InboxItems = new List<GameObject>();

        int verticalOffset = 0;
        foreach (Item item in PlayerInventory.Inbox)
        {
            GameObject newInboxItem = Instantiate(InboxItemPrefab, InboxContent.transform);
            InboxItems.Add(newInboxItem);
            newInboxItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, verticalOffset);
            verticalOffset -= 74;

            newInboxItem.GetComponent<InboxItemController>().Initialize(this, item);
        }

        InboxContent.GetComponent<RectTransform>().sizeDelta = new Vector2(InboxItemPrefab.GetComponent<RectTransform>().sizeDelta.x, -1 * verticalOffset);
    }

    private void RefreshStorage()
    {
        Vector2 sizeDelta = StorageContent.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = PlayerInventory.Storage.GetLength(2) * 74 - 10;
        StorageContent.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        int page = 0;
        for (int col = 0; col < PlayerInventory.Storage.GetLength(1); col++)
        {
            for (int row = 0; row < PlayerInventory.Storage.GetLength(2); row++)
            {
                GameObject newStorageSlot = Instantiate(StorageSlotPrefab, StorageContent.transform);
                newStorageSlot.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                newStorageSlot.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                newStorageSlot.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                newStorageSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(col * 74, - row * 74);
                newStorageSlot.GetComponent<StorageSlotController>().Initialize(this, col, row, PlayerInventory.Storage[page, col, row]);
            }
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

    public bool ItemSelected(Item item, GameObject icon)
    {
        if (CursorItem == null)
        {
            CursorItem = item;
            CursorIcon = icon;

            CursorIcon.transform.SetParent(GameManager.Instance.RootCanvas.transform);
            CursorIcon.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ItemRequestedByStorage(int col, int row)
    {
        if (CursorItem != null)
        {
            int page = 0;
            PlayerInventory.MoveItemToStorage(CursorItem, page, col, row);
            CursorItem = null;
            Destroy(CursorIcon);
            RefreshStorage();
        }
    }
}
