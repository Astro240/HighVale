using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
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

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform missing on InventoryItem.");
            return;
        }

        // Don't use FindObjectOfType here anymore. This is now injected externally.
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

        // Ensure rectTransform is initialized
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


    private void OnMouseDown()
    {
        if (rectTransform != null)
        {
            offset = rectTransform.position - (Vector3)Input.mousePosition;
            isDragging = true;
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging && rectTransform != null)
        {
            rectTransform.position = Input.mousePosition + (Vector3)offset;
        }
    }

    private void OnMouseUp()
    {
        if (itemGrid != null)
        {
            Vector2Int gridPosition = itemGrid.GetTileGridPosition(Input.mousePosition);

            if (itemGrid.IsValidGridPosition(gridPosition.x, gridPosition.y))
            {
                itemGrid.PlaceItem(this, gridPosition.x, gridPosition.y);
            }
            else
            {
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
