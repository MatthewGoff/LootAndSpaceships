using UnityEngine;

public class InboxEntryController : MonoBehaviour
{
    public GameObject ItemIconPrefab;

    private InventoryGUIController InventoryGUIController;
    private Item Item;
    private int InboxIndex;
    private float TargetHeight;

    public void Initialize(InventoryGUIController inventoryGUIController, Item item, int inboxIndex)
    {
        InventoryGUIController = inventoryGUIController;
        Item = item;
        InboxIndex = inboxIndex;
        TargetHeight = GetComponent<RectTransform>().anchoredPosition.y;

        GameObject gameObject = Instantiate(ItemIconPrefab, transform);
        gameObject.GetComponent<ItemIconController>().Initialize(ItemSprites.Instance.GetItemSprites(Item.ItemType), Item.Colors);
    }

    private void Update()
    {
        float distance = TargetHeight - GetComponent<RectTransform>().anchoredPosition.y;
        if (distance != 0)
        {
            float step = 10 * distance * Time.deltaTime;
            if (step < 1)
            {
                step = 1;
            }
            if (step > distance)
            {
                step = distance;
            }
            GetComponent<RectTransform>().anchoredPosition += new Vector2(0, step);
        }
    }

    public void Keep()
    {
        InventoryGUIController.Keep(Item, InboxIndex);
    }

    public void Scrap()
    {
        InventoryGUIController.Scrap(Item, InboxIndex);
    }

    public void MoveUp()
    {
        InboxIndex--;
        TargetHeight += 74;
    }
}
