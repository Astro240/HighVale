using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemGrid selectedItemGrid;
    private InventoryItem selectedItem;
    private InventoryItem overlapItem;
    private RectTransform rectTransform;

    [SerializeField] private List<ItemData> items;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private GameObject inventoryCanvas;

    private bool inventoryVisible = true;
    private InventoryHighlight inventoryHighlight;

    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            if (inventoryHighlight != null)
            {
                inventoryHighlight.SetParent(value);
            }
        }
    }

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();

        if (selectedItemGrid == null)
        {
            selectedItemGrid = FindObjectOfType<ItemGrid>();
            if (selectedItemGrid == null)
            {
                Debug.LogError("No ItemGrid found in the scene!");
            }
        }

        // Ensure the property setter logic runs
        SelectedItemGrid = selectedItemGrid;
    }

    private void Start()
    {
        inventoryVisible = false;

        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Inventory canvas is not assigned in the inspector!");
        }
    }

    private void Update()
    {
        // Toggle inventory visibility with I key
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryVisible = !inventoryVisible;
            inventoryCanvas.SetActive(inventoryVisible);
        }

        // Create a random item on Q press
        if (Input.GetKeyDown(KeyCode.Q) && selectedItem == null)
        {
            CreateRandomItem();
        }

        // Rotate selected item on R press
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        if (SelectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        HandleHighlight();

        // Handle Left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }

        // Handle item dragging if selectedItem is not null
        if (selectedItem != null && Input.GetMouseButton(0))
        {
            DragItem();
        }
    }

    private void RotateItem()
    {
        if (selectedItem != null)
        {
            selectedItem.Rotate();
        }
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = SelectedItemGrid.FindSpaceForObject(itemToInsert);

        if (!posOnGrid.HasValue) return;

        SelectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    private Vector2Int oldPosition;
    private InventoryItem itemToHighlight;

    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPosition == positionOnGrid) return;

        oldPosition = positionOnGrid;

        if (selectedItem == null)
        {
            if (SelectedItemGrid.IsValidGridPosition(positionOnGrid.x, positionOnGrid.y))
            {
                itemToHighlight = SelectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            }
            else
            {
                itemToHighlight = null;
            }

            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(SelectedItemGrid, itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            // Correct method name 'BoundaryCheck' instead of 'BoundryCheck'
            bool canPlace = SelectedItemGrid.BoundaryCheck(positionOnGrid.x, positionOnGrid.y, selectedItem.WIDTH, selectedItem.HEIGHT);
            inventoryHighlight.Show(canPlace);
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetPosition(SelectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }


    private void CreateRandomItem()
    {
        if (items.Count == 0)
        {
            Debug.LogWarning("No items in the list to create!");
            return;
        }

        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }

        Vector2Int gridPos = SelectedItemGrid.GetTileGridPosition(position);

        // Clamp to avoid out-of-bounds access
        gridPos.x = Mathf.Clamp(gridPos.x, 0, SelectedItemGrid.GridSizeWidth - 1);
        gridPos.y = Mathf.Clamp(gridPos.y, 0, SelectedItemGrid.GridSizeHeight - 1);

        return gridPos;
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = SelectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (complete)
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                Destroy(overlapItem.gameObject);
            }
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        InventoryItem item = SelectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (item != null)
        {
            selectedItem = item;
        }
    }

    private void DragItem()
    {
        // Move the selected item with the mouse cursor.
        if (selectedItem != null)
        {
            Vector2 mousePosition = Input.mousePosition;
            RectTransform selectedRectTransform = selectedItem.GetComponent<RectTransform>();
            selectedRectTransform.position = mousePosition;
        }
    }
}