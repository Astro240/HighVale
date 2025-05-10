using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 32f;
    public const float tileSizeHeight = 32f;

    public static float TileSizeWidth => tileSizeWidth;
    public static float TileSizeHeight => tileSizeHeight;

    private InventoryItem[,] inventoryItemSlot;
    private RectTransform recTransform;

    [SerializeField] private int gridSizeWidth = 20;
    [SerializeField] private int gridSizeHeight = 10;

    private void Start()
    {
        recTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }

    public int GridSizeWidth => gridSizeWidth;
    public int GridSizeHeight => gridSizeHeight;

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        recTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        Vector2 localMousePosition = mousePosition - (Vector2)recTransform.position;
        int x = Mathf.FloorToInt(localMousePosition.x / tileSizeWidth);
        int y = Mathf.FloorToInt(-localMousePosition.y / tileSizeHeight);
        return new Vector2Int(x, y);
    }

    public bool IsValidGridPosition(int posX, int posY)
    {
        return posX >= 0 && posY >= 0 && posX < gridSizeWidth && posY < gridSizeHeight;
    }

    public bool BoundaryCheck(int posX, int posY, int width, int height)
    {
        if (!PositionCheck(posX, posY)) return false;
        posX += width - 1;
        posY += height - 1;
        return PositionCheck(posX, posY);
    }

    private bool PositionCheck(int posX, int posY)
    {
        return posX >= 0 && posY >= 0 && posX < gridSizeWidth && posY < gridSizeHeight;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        if (!IsValidGridPosition(x, y)) return null;

        InventoryItem item = inventoryItemSlot[x, y];
        if (item != null)
        {
            CleanGridReference(item);
        }

        return item;
    }

    private void CleanGridReference(InventoryItem item)
    {
        for (int x = 0; x < item.WIDTH; x++)
        {
            for (int y = 0; y < item.HEIGHT; y++)
            {
                inventoryItemSlot[item.onGridPostionX + x, item.onGridPostionY + y] = null;
            }
        }
    }

    // Updated PlaceItem method with ref InventoryItem overlapItem
    public bool PlaceItem(InventoryItem item, int posX, int posY, ref InventoryItem overlapItem)
    {
        // Ensure the item can fit within the grid at the given position
        if (!BoundaryCheck(posX, posY, item.WIDTH, item.HEIGHT))
        {
            overlapItem = null;
            return false;
        }

        // Check for overlap and set the overlapItem reference if there is one
        if (!OverlapCheck(posX, posY, item.WIDTH, item.HEIGHT, ref overlapItem))
        {
            overlapItem = null;
            return false;
        }

        // If there's an overlap, clean up the overlap item
        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        // Place the item on the grid
        PlaceItem(item, posX, posY); // This assumes the old PlaceItem method will still work for placement
        return true;
    }

    // This is the original method that places an item without overlap checking
    public void PlaceItem(InventoryItem item, int posX, int posY)
    {
        RectTransform rt = item.GetComponent<RectTransform>();
        rt.SetParent(recTransform);

        for (int x = 0; x < item.WIDTH; x++)
        {
            for (int y = 0; y < item.HEIGHT; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = item;
            }
        }

        item.onGridPostionX = posX;
        item.onGridPostionY = posY;
        rt.localPosition = CalculatePositionOnGrid(item, posX, posY);
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem item, int posX, int posY)
    {
        return new Vector2(
            posX * tileSizeWidth + tileSizeWidth * item.WIDTH / 2,
            -(posY * tileSizeHeight + tileSizeHeight * item.HEIGHT / 2)
        );
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var item = inventoryItemSlot[posX + x, posY + y];
                if (item != null)
                {
                    if (overlapItem == null) overlapItem = item; // First overlap
                    else if (overlapItem != item) return false;  // Multiple items found, return false
                }
            }
        }
        return true;  // No overlap or only one item found
    }

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                    return false;
            }
        }
        return true;
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        int height = gridSizeHeight - itemToInsert.HEIGHT + 1;
        int width = gridSizeWidth - itemToInsert.WIDTH + 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y, itemToInsert.WIDTH, itemToInsert.HEIGHT))
                    return new Vector2Int(x, y);
            }
        }

        return null;
    }

    internal InventoryItem GetItem(int x, int y)
    {
        if (!IsValidGridPosition(x, y)) return null;
        return inventoryItemSlot[x, y];
    }
}
