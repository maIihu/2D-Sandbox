using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapRenderer : MonoBehaviour
{
    public Tilemap tilemap;              // Tilemap để vẽ lên
    public TileBase groundTile;          // Tile đất
    public TileBase waterTile;           // Tile nước

    public void RenderChunk(Vector3Int chunkPosition)
    {
        // Vẽ tiles cho chunk này
        for (int x = 0; x < 16; x++) // Giới hạn kích thước chunk là 16x16 tiles
        {
            for (int y = 0; y < 16; y++)
            {
                TileBase tile = Mathf.PerlinNoise((float)(chunkPosition.x + x) / 100f, (float)(chunkPosition.y + y) / 100f) < 0.4f ? waterTile : groundTile;
                tilemap.SetTile(new Vector3Int(chunkPosition.x + x, chunkPosition.y + y, 0), tile);
            }
        }
    }
}