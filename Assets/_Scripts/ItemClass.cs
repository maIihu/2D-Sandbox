using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ItemClass
{
    public enum ItemType
    {
        block,
        tool
    };

    public enum ToolType
    {
        pick,
        sword
    };

    public ItemType itemType;
    public ToolType toolType;

    public TileClass tile;
    public ToolClass tool;

    public string itemName;
    public Sprite itemSprite;
    public bool isStackable;

    public ItemClass(TileClass tileClass)
    {
        this.itemName = tileClass.tileName;

        if (tileClass.tileSprite is Tile tile)
        {
            this.itemSprite = tile.sprite;
        }

        isStackable = true;
        itemType = ItemType.block;
        this.tile = tileClass;
    }

    public ItemClass(ToolClass toolClass)
    {
        this.itemName = toolClass.name;
        this.itemSprite = toolClass.toolSprite;
        isStackable = false;
        itemType = ItemType.tool;
        toolType = toolClass.toolType;
        this.tool = toolClass;
    }
    
}
