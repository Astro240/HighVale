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

        GameObject itemGO = Instantiate(inventoryItemPrefab);
        RectTransform rt = itemGO.GetComponent<RectTransform>();
        rt.SetParent(inventoryController.SelectedItemGrid.transform, false);

        InventoryItem inventoryItem = itemGO.GetComponent<InventoryItem>();
        if (inventoryItem == null)
        {
            Debug.LogWarning("InventoryItem script not found on prefab!");
            Destroy(itemGO);
            return;
        }

        // Inject the ItemGrid before calling Set()
        inventoryItem.SetGrid(inventoryController.SelectedItemGrid);
        inventoryItem.Set(catItemData);

        Vector2Int? pos = inventoryController.SelectedItemGrid.FindSpaceForObject(inventoryItem);
        if (pos.HasValue)
        {
            inventoryController.SelectedItemGrid.PlaceItem(inventoryItem, pos.Value.x, pos.Value.y);
            rt.localPosition = inventoryController.SelectedItemGrid.CalculatePositionOnGrid(inventoryItem, pos.Value.x, pos.Value.y);

            // ADD THIS CODE HERE:  Reset cursor state
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;

            Destroy(gameObject); // Remove pickup object from world
        }
        else
        {
            Destroy(itemGO); // Not enough space
            Debug.Log("Not enough space in inventory!");
        }
    }
}
