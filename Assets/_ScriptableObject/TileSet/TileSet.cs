using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileSet", menuName = "TileSet")]
public class TileSet : ScriptableObject
{
    public TileClass emptyTile;
    public TileClass dirtGrassTile;
    public TileClass dirtTile;
    public TileClass trunkTile;
    public TileClass leaveTile;
}
