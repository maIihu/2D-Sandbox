using System;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Inventory : MonoBehaviour
{
    public InventorySlot[,] inventorySlots;
    public InventorySlot[] hotbarSlots;
    
    public GameObject[,] iventoryUISlots;
    public GameObject[] hotbarUISlot ;
    
    public int inventoryWidth;
    public int inventoryHeight;
    
    public GameObject inventoryUI;
    public GameObject inventorySlotPrefab;
    public GameObject hotbarUI;

    public Vector2 inventoryOffset;
    public Vector2 hotbarOffset;
    public Vector2 multiphier;

    [FormerlySerializedAs("ToolClass")] public ToolClass Pick;
    public TileClass TileClass;
    public ToolClass Sword;

    private void Awake()
    {
        inventorySlots = new InventorySlot[inventoryWidth, inventoryHeight];
        iventoryUISlots = new GameObject[inventoryWidth, inventoryHeight];
        hotbarSlots = new InventorySlot[inventoryWidth];
        hotbarUISlot = new GameObject[inventoryWidth];
    }

    private void Start()
    {
        
        SetUpInventoryUI();
        UpdateInventoryUI();
        
        AddItem(new ItemClass(Pick));
        AddItem(new ItemClass(Sword));
        AddItem(new ItemClass(TileClass));
        
    }

    private void Update()
    {
    }

    private void SetUpInventoryUI()
    {
        // invent
        for (int x = 0; x < inventoryWidth; x++)
        {
            for (int y = 0; y < inventoryHeight; y++)
            {
                GameObject inventory = Instantiate(inventorySlotPrefab, inventoryUI.transform.GetChild(0).transform);
                inventory.GetComponent<RectTransform>().localPosition = new Vector3((x * multiphier.x) + inventoryOffset.x, (y*multiphier.y) + inventoryOffset.y);
                iventoryUISlots[x, y] = inventory;
                inventorySlots[x, y] = null;
            }
        }
        // hotbar
        for (int x = 0; x < inventoryWidth; x++)
        {
            GameObject hotbar = Instantiate(inventorySlotPrefab, hotbarUI.transform.GetChild(0).transform);
            hotbar.GetComponent<RectTransform>().localPosition = new Vector3((x * multiphier.x) + hotbarOffset.x, hotbarOffset.y);
            hotbarUISlot[x] = hotbar;
            hotbarSlots[x] = null;
        }
    }
    
    private void UpdateInventoryUI()
    {
        // invent
        for (int x = 0; x < inventoryWidth; x++)
        {
            for (int y = 0; y < inventoryHeight; y++)
            {
                if (inventorySlots[x, y] == null)
                {
                    iventoryUISlots[x, y].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    iventoryUISlots[x, y].transform.GetChild(0).GetComponent<Image>().enabled = false;
                    
                    iventoryUISlots[x, y].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
                    iventoryUISlots[x, y].transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = false;
                }
                else
                {
                    iventoryUISlots[x, y].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    iventoryUISlots[x, y].transform.GetChild(0).GetComponent<Image>().sprite = inventorySlots[x,y].item.itemSprite;
                    
                    iventoryUISlots[x, y].transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = true;
                    iventoryUISlots[x, y].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inventorySlots[x,y].quantity.ToString();
                }
            }
        }
        
        // hotbar
        for (int x = 0; x < inventoryWidth; x++)
        {
            if (inventorySlots[x, inventoryHeight - 1] == null)
            {
                hotbarUISlot[x].transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarUISlot[x].transform.GetChild(0).GetComponent<Image>().enabled = false;
                
                hotbarUISlot[x].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
                hotbarUISlot[x].transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = false;
            }
            else
            {
                hotbarUISlot[x].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarUISlot[x].transform.GetChild(0).GetComponent<Image>().sprite = inventorySlots[x, inventoryHeight - 1].item.itemSprite;

                hotbarUISlot[x].transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = true;
                if(inventorySlots[x, inventoryHeight - 1].item.tile)
                    hotbarUISlot[x].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inventorySlots[x, inventoryHeight - 1].quantity.ToString();
                else 
                    hotbarUISlot[x].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            
        }
    }

    public bool AddItem(ItemClass item)
    {
        bool added = false;
        Vector2Int itemPos = Contain(item);
        if (itemPos != Vector2Int.one * -1)
        {
            inventorySlots[itemPos.x, itemPos.y].quantity++;
            added = true;
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
                if (inventorySlots[x, y] != null && inventorySlots[x, y].item.itemSprite == item.itemSprite)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return Vector2Int.one * -1;
    }

    public void RemoveItem(ItemClass item)
    {
        Vector2Int itemPos = Contain(item);
        if (itemPos != Vector2Int.one * -1)
        {
            if (inventorySlots[itemPos.x, itemPos.y].quantity > 0)
            {
                inventorySlots[itemPos.x, itemPos.y].quantity--;
                UpdateInventoryUI();
            }

            if (inventorySlots[itemPos.x, itemPos.y].quantity <= 0)
            {
                inventorySlots[itemPos.x, itemPos.y] = null;
                UpdateInventoryUI();
            }
            //Debug.Log("Hello");
        }
    }
}
