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

    public string name;
    public Sprite sprite;
    public bool isStackable;

    public ItemClass(TileClass tileBase)
    {
        this.name = tileBase.tileName;

        if (tileBase.tileSprite is Tile tile)
        {
            this.sprite = tile.sprite;
        }

        isStackable = true;
        itemType = ItemType.block;
    }

    public ItemClass(ToolClass toolClass)
    {
        this.name = toolClass.name;
        this.sprite = toolClass.toolSprite;
        isStackable = false;
        itemType = ItemType.tool;
        toolType = toolClass.toolType;
    }
    
}
