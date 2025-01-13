using UnityEngine.Tilemaps;

public class TilemapInfo
{
    public Tilemap Tilemap { get; set; }
    
    public int PosX { get; set; }

    public TilemapInfo(Tilemap tilemap, int posX)
    {
        this.Tilemap = tilemap;
        this.PosX = posX;
    }
}