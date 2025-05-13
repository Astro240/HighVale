using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData itemData;

    public int onGridPostionX;
    public int onGridPostionY;

    public bool rotated = false;

    public int HEIGHT => itemData != null ? (rotated ? itemData.width : itemData.height) : 0;
    public int WIDTH => itemData != null ? (rotated ? itemData.height : itemData.width) : 0;

    private Vector2 offset;
    private RectTransform rectTransform;
    [HideInInspector] public ItemGrid itemGrid; // Now set externally
    private bool isDragging = false;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void Start()
    {
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform missing on InventoryItem.");
            return;
        }

        if (itemGrid == null)
        {
            Debug.LogWarning("ItemGrid was not injected. Please assign it manually.");
        }

        if (itemData != null)
        {
            Set(itemData);
        }
    }

    public void SetGrid(ItemGrid grid)
    {
        itemGrid = grid;
    }

    internal void Set(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogError("ItemData is null in Set().");
            return;
        }

        this.itemData = itemData;

        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError("RectTransform missing on InventoryItem.");
                return;
            }
        }

        Image itemImage = GetComponent<Image>();
        if (itemImage != null)
        {
            itemImage.sprite = itemData.itemIcon;
        }
        else
        {
            Debug.LogError("Image component missing on InventoryItem.");
        }

        if (itemData.width <= 0 || itemData.height <= 0)
        {
            Debug.LogError("Invalid itemData dimensions.");
            return;
        }

        Vector2 size = new Vector2(
            itemData.width * ItemGrid.TileSizeWidth,
            itemData.height * ItemGrid.TileSizeHeight
        );
        rectTransform.sizeDelta = size;

        onGridPostionX = 0;
        onGridPostionY = 0;

        if (itemGrid != null)
        {
            rectTransform.localPosition = itemGrid.CalculatePositionOnGrid(this, onGridPostionX, onGridPostionY);
        }
        else
        {
            Debug.LogWarning("ItemGrid is not set during Set(). Positioning may be incorrect.");
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemGrid == null) return;

        isDragging = true;
        offset = rectTransform.position - (Vector3)Input.mousePosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        itemGrid.PickUpItem(onGridPostionX, onGridPostionY);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform != null)
        {
            rectTransform.position = Input.mousePosition + (Vector3)offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (itemGrid != null)
        {
            Vector2Int gridPosition = itemGrid.GetTileGridPosition(Input.mousePosition);

            if (itemGrid.IsValidGridPosition(gridPosition.x, gridPosition.y))
            {
                itemGrid.PlaceItem(this, gridPosition.x, gridPosition.y);
            }
            else
            {
                itemGrid.PlaceItem(this, onGridPostionX, onGridPostionY);
                rectTransform.localPosition = itemGrid.CalculatePositionOnGrid(this, onGridPostionX, onGridPostionY);
            }
        }
    }

    internal void Rotate()
    {
        if (itemGrid == null) return;

        rotated = !rotated;
        rectTransform.rotation = Quaternion.Euler(0, 0, rotated ? 90f : 0f);
        itemGrid.PlaceItem(this, onGridPostionX, onGridPostionY);
    }
}