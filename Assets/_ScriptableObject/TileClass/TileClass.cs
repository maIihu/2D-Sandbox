using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileClass", menuName = "TileClass")]
public class TileClass : ScriptableObject
{
    public string tileName;
    public TileBase tileSprite;
}
