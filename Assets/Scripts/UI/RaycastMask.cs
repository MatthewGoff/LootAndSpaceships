using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class RaycastMask : MonoBehaviour, ICanvasRaycastFilter
{
    private Sprite Sprite;

    void Start()
    {
        Sprite = GetComponent<Image>().sprite;
    }

    public bool IsRaycastLocationValid(Vector2 screenPosition, Camera raycastCamera)
    {
        if (Sprite == null)
        {
            return true;
        }
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPosition, raycastCamera, out localPosition);
        // normalize local coordinates
        Vector2 normalized = new Vector2(
            (localPosition.x + rectTransform.pivot.x * rectTransform.rect.width) / rectTransform.rect.width,
            (localPosition.y + rectTransform.pivot.y * rectTransform.rect.height) / rectTransform.rect.height);
        // convert to texture space
        Rect rect = Sprite.rect;
        int x = Mathf.FloorToInt(rect.x + rect.width * normalized.x);
        int y = Mathf.FloorToInt(rect.y + rect.height * normalized.y);
        // destroy component if texture import settings are wrong
        try
        {
            return Sprite.texture.GetPixel(x, y).a > 0;
        }
        catch
        {
            Debug.LogError("Mask texture not readable, set your sprite to Texture Type 'Advanced' and check 'Read/Write Enabled'");
            Destroy(this);
            return false;
        }
    }
}