using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    private ItemGrid selectedItemGrid;
    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform  rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;



    

    InventoryHighlight inventoryHighlight;
    public ItemGrid SelectedItemGrid { 
        get => selectedItemGrid; 
        set
        {

            selectedItemGrid = value;
            inventoryHighlight.SetParent(value); 
        } 
    }

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }
    private void Update()
    {
        ItemIconDrag();

      

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedItem == null)
            {
                CreateRandomItem();
            }
        }

        if(Input.GetKeyDown(KeyCode.W)) 
        {
            InsertRandomItem();
        }
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

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }





    }

    private void RotateItem()
    {
        if (selectedItem == null) { return; }

        selectedItem.Rotate(); 

        
    }

    private void InsertRandomItem()
    {
        if (selectedItemGrid == null)  { return; }
            CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        
        
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

        if(posOnGrid == null) { return; }

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    Vector2Int oldPosition;
    InventoryItem itemToHighlight;



    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPosition == positionOnGrid) { return; } 
        
        
        
        oldPosition = positionOnGrid;
        if (selectedItem == null)
        {

            itemToHighlight = SelectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
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
            inventoryHighlight.Show(SelectedItemGrid.BoundryCheck(
                positionOnGrid.x,
                positionOnGrid.y, 
                selectedItem.WIDTH,
                selectedItem.HEIGHT) 
                
                );
            inventoryHighlight.SetSize(selectedItem);
           
            inventoryHighlight.SetPosition(SelectedItemGrid, selectedItem,  positionOnGrid.x, positionOnGrid.y);


        }
    }

    private void CreateRandomItem()
    {
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
            position.x -= (selectedItem.WIDTH- 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;




        }


        
        return SelectedItemGrid.GetTileGridPosition(position);
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
       bool complete =  SelectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem );
        if (complete) 
        {
            selectedItem = null;
            if (overlapItem != null) 
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }

        }
       
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = SelectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {

            rectTransform.position = Input.mousePosition;
        }
    }


}

