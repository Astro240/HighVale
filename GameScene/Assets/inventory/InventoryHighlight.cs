using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField]  RectTransform highlighter;

    public void Show(bool b) 
    {
    highlighter.gameObject.SetActive(b); 
    
    }
    public void SetSize (InventoryItem targetItem)
    {

        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * ItemGrid.tileSizeWidth;
        size.y = targetItem.HEIGHT * ItemGrid.tileSizeHeight;
        highlighter.sizeDelta = size;

    }
    public void  SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        

        Vector2 pos = targetGrid.CalculatePositioOnGrid(
            targetItem,
            targetItem.onGridPostionX,
            targetItem.onGridPostionY);
        highlighter.localPosition = pos;


    }

    public void SetParent(ItemGrid targetGrid)
    {
        if (targetGrid == null) { return; }
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem,int posX,int posY) 
    
   {
        Vector2 pos = targetGrid.CalculatePositioOnGrid(
            targetItem,
            posX,
            posY
            );

        highlighter.localPosition = pos;
    
    
    }
}
