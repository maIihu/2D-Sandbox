using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapRenderer : MonoBehaviour
{
    public TileBase groundTile;      
    public TileBase waterTile;          
    
    public void RenderChunk(Vector3Int chunkPosition, Tilemap tilemap)
    {
        for (int x = 0; x < 16; x++) 
        {
            for (int y = 0; y < 16; y++)
            {
                TileBase tile = Mathf.PerlinNoise((float)(chunkPosition.x + x) / 100f, (float)(chunkPosition.y + y) / 100f) < 0.4f ? waterTile : groundTile;
                
                Vector3Int tilePosition = new Vector3Int(chunkPosition.x + x, chunkPosition.y + y, 0);
                tilemap.SetTile(tilePosition, tile);
            }
        }
    }
}
