using UnityEngine;
using UnityEngine.EventSystems;

public class CatPickup : MonoBehaviour
{
    public ItemData catItemData;
    public GameObject inventoryItemPrefab;
    public float interactionRange = 2f;

    private Transform player;
    private InventoryController inventoryController;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        inventoryController = FindObjectOfType<InventoryController>();

        if (player == null)
        {
            Debug.LogError("Player not found!");
        }

        if (inventoryController == null)
        {
            Debug.LogError("InventoryController not found!");
        }

        if (catItemData == null)
        {
            Debug.LogError("catItemData is not assigned in the Inspector!");
        }
    }

    private void Update()
    {
        if (player == null || inventoryController == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= interactionRange)
        {
            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetKeyDown(KeyCode.F))
            {
                TryAddToInventory();
            }
        }
    }

    private void TryAddToInventory()
    {
        if (inventoryController.SelectedItemGrid == null)
        {
            Debug.LogWarning("No inventory grid selected.");
            return;
        }

        if (catItemData == null)
        {
            Debug.LogError("catItemData is null! Cannot add item to inventory.");
            return;
        }

        // Instantiate the item GameObject
        // Instantiate the item GameObject
        // Instantiate the item GameObject
        GameObject itemGO = Instantiate(inventoryItemPrefab);
        InventoryItem inventoryItem = itemGO.GetComponent<InventoryItem>();

        if (inventoryItem == null)
        {
            Debug.LogWarning("InventoryItem script not found on prefab!");
            Destroy(itemGO);
            return;
        }

        // Set grid before Set()
        inventoryItem.SetGrid(inventoryController.SelectedItemGrid);

        // Now assign item data
        inventoryItem.Set(catItemData);

        // Find a spot BEFORE adding to grid
        Vector2Int? pos = inventoryController.SelectedItemGrid.FindSpaceForObject(inventoryItem);
        if (pos.HasValue)
        {
            itemGO.transform.SetParent(inventoryController.SelectedItemGrid.transform, false);

            inventoryController.SelectedItemGrid.PlaceItem(inventoryItem, pos.Value.x, pos.Value.y);

            RectTransform rt = itemGO.GetComponent<RectTransform>();
            rt.localPosition = inventoryController.SelectedItemGrid.CalculatePositionOnGrid(inventoryItem, pos.Value.x, pos.Value.y);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Destroy(gameObject); // Destroy pickup object
        }
        else
        {
            Destroy(itemGO);
            Debug.Log("Not enough space in inventory!");
        }

    }
}



