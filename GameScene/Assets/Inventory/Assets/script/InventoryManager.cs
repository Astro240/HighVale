using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public Toggle EnableRemove; 


    public static InventoryManager Instance;

    [Header("UI References")]
    public Transform Content;            // Parent object to hold inventory items (like Content under a ScrollView)
    public GameObject Item;         // Prefab representing a single inventory item

    [Header("Inventory Data")]
    public List<Item> Items = new List<Item>();

    private void Awake()
    {
        Instance = this;

        // Debug check for setup issues
        if (Item == null)
        {
            Debug.LogError("❌ InventoryItem prefab is not assigned in the Inspector!");
        }

        if (Content == null)
        {
            Debug.LogError("❌ ItemContent Transform is not assigned in the Inspector!");
        }
    }

    public void Add(Item item)
    {
        if (item == null)
        {
            Debug.LogError("❌ Tried to add a null item to the inventory.");
            return;
        }

        if (Items.Contains(item))
        {
            Debug.LogWarning("⚠️ Item already in inventory: " + item.itemName);
            return;
        }

        Items.Add(item);
        Debug.Log("✅ Added to inventory: " + item.itemName);
        ListItems(); // Refresh UI
    }

    public void Remove(Item item)
    {
        if (item == null)
        {
            Debug.LogError("❌ Tried to remove a null item from the inventory.");
            return;
        }

        Items.Remove(item);
        ListItems(); // Refresh UI
    }

    public void ListItems()
    {
        if (Content == null || Item == null)
        {
            Debug.LogError("❌ Inventory UI is not fully assigned.");
            return;
        }

        // Clear previous items
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }

        // Rebuild inventory UI
        foreach (var item in Items)
        {
            if (item == null)
            {
                Debug.LogWarning("⚠️ Skipped null item in inventory.");
                continue;
            }

            GameObject obj = Instantiate(Item, Content);

            if (obj == null)
            {
                Debug.LogError("❌ Failed to instantiate InventoryItem prefab.");
                continue;
            }

            Transform itemName = obj.transform.Find("ItemName");
            Transform itemIcon = obj.transform.Find("ItemIcon");

            if (itemName == null || itemIcon == null)
            {
                Debug.LogError("❌ Missing child GameObjects: ItemName or ItemIcon.");
                Destroy(obj);
                continue;
            }

            TMP_Text itemNameText = itemName.GetComponent<TMP_Text>();
            Image itemIconImage = itemIcon.GetComponent<Image>();

            if (itemNameText == null || itemIconImage == null)
            {
                Debug.LogError("❌ Missing TMP_Text or Image components.");
                Destroy(obj);
                continue;
            }

            itemNameText.text = item.itemName;
            itemIconImage.sprite = item.icon;
        }
    }

    public void EnableItemsRemove()
    {
        if(EnableRemove.isOn)
        {
            foreach(Transform item in Content) 
            { 
                item.Find("RemoveButton").gameObject.SetActive(true);
            }
        }else
        {
            foreach (Transform item in Content)
            {
                item.Find("RemoveButton").gameObject.SetActive(false);
            }


        }
    }



}
