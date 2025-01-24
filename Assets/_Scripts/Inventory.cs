using System;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Inventory : MonoBehaviour
{
    public InventorySlot[,] inventorySlots;
    public GameObject[,] uiSlots;
    
    public int inventoryWidth;
    public int inventoryHeight;
    
    public GameObject inventoryUI;
    public GameObject inventorySlotPrefab;

    public Vector2 offset;
    public Vector2 multiphier;

    public ToolClass ToolClass;
    public TileClass TileClass;

    public int stackLimit = 30;
    
    private void Start()
    {
        inventorySlots = new InventorySlot[inventoryWidth, inventoryHeight];
        uiSlots = new GameObject[inventoryWidth, inventoryHeight];
        SetUpInventoryUI();
        UpdateInventoryUI();
        AddItem(new ItemClass(ToolClass));
        AddItem(new ItemClass(TileClass));
    }

    private void Update()
    {
    }

    private void SetUpInventoryUI()
    {
        for (int x = 0; x < inventoryWidth; x++)
        {
            for (int y = 0; y < inventoryHeight; y++)
            {
                GameObject inventory = Instantiate(inventorySlotPrefab, inventoryUI.transform.GetChild(0).transform);
                inventory.GetComponent<RectTransform>().localPosition = new Vector3((x * multiphier.x) + offset.x, (y*multiphier.y) + offset.y);
                uiSlots[x, y] = inventory;
                inventorySlots[x, y] = null;
            }
        }
    }

    private void UpdateInventoryUI()
    {
        for (int x = 0; x < inventoryWidth; x++)
        {
            for (int y = 0; y < inventoryHeight; y++)
            {
                if (inventorySlots[x, y] == null)
                {
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().enabled = false;
                    
                    uiSlots[x, y].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
                    uiSlots[x, y].transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = false;
                }
                else
                {
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().sprite = inventorySlots[x,y].item.sprite;
                    
                    uiSlots[x, y].transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = true;
                    uiSlots[x, y].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inventorySlots[x,y].quantity.ToString();
                }
            }
        }
    }

    public bool AddItem(ItemClass item)
    {
        bool added = false;
        Vector2Int itemPos = Contain(item);
        if (itemPos != Vector2Int.one * -1)
        {
            if (inventorySlots[itemPos.x, itemPos.y].quantity < stackLimit)
            {
                inventorySlots[itemPos.x, itemPos.y].quantity++;
                added = true;
            }
        }
        if (!added)
        {
            for (int y = inventoryHeight - 1; y >= 0; y--)
            {
                if (added) break;
                for (int x = 0; x < inventoryWidth; x++)
                {
                    if (inventorySlots[x, y] == null)
                    {
                        inventorySlots[x, y] = new InventorySlot{item = item, pos = new Vector2Int(x, y), quantity = 1};
                        added = true;
                        break;
                    }
                }
            }
        }
        UpdateInventoryUI();
        return added;
    }

    private Vector2Int Contain(ItemClass item)
    {
        for (int y = inventoryHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                if (inventorySlots[x, y] != null && inventorySlots[x, y].item.sprite == item.sprite)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return Vector2Int.one * -1;
    }
    
}
