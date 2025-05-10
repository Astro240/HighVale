using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item; // Reference to the item being picked up
    private bool isPickedUp = false; // Prevent multiple pickups

    void Pickup()
    {
        if (isPickedUp)
            return;

        isPickedUp = true;
        Debug.Log("Item picked up: " + Item.itemName);
        InventoryManager.Instance.Add(Item); // Add item to inventory
        Destroy(gameObject); // Destroy the item in the game world
    }

    private void OnMouseDown()
    {
        Pickup(); // Call Pickup when the item is clicked
    }

    void Update()
    {
        if (isPickedUp) return; // Don't do anything if already picked up

        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    Pickup(); // Call Pickup if the item is clicked
                }
            }
        }
    }
}
