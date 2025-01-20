using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MapRenderer : MonoBehaviour
{
    public TileBase emptyTile;
    public TileBase dirtSurfaceTile;
    public TileBase dirtTile;
    public TileBase treeTile;  
    
    public int height = 100;
    public int width = 16;
    
    public float terrainScale = 20f;
    public float caveScale = 10f;
    public float caveThreshold = 0.2f;
    public float treeProbability = 0.1f; 
    
    public int seed;
    
    private void Start()
    {
        seed = Random.Range(-100000, 100000);
    }
    
    public void RenderChunk(Vector3Int chunkPosition, Tilemap tilemap)
    {
        for (int x = 0; x < width; x++) 
        {
            int groundHeight = Mathf.FloorToInt(Mathf.PerlinNoise((x + chunkPosition.x) / terrainScale, seed) * height / 2 + height / 4);
            for (int y = 0; y <= groundHeight; y++)
            {
                TileBase tile;
                float noiseValue = Mathf.PerlinNoise((x + chunkPosition.x) / caveScale + seed, (y + chunkPosition.y) / caveScale + seed);
            
                if (y == 0)
                {
                    tile = dirtTile;
                }
                else if (noiseValue < caveThreshold)
                {
                    tile = emptyTile;
                }
                else if (y == groundHeight)
                {
                    tile = dirtSurfaceTile;
                }
                else
                {
                    tile = dirtTile;
                }
            
                Vector3Int tilePosition = new Vector3Int(x, chunkPosition.y + y, 0);
                tilemap.SetTile(tilePosition, tile);
                
                if (y == groundHeight && tile == dirtSurfaceTile)
                {
                    float treeChance = Random.value;
                    if (treeChance < treeProbability)
                    {
                        int trunkHeight = Random.Range(3, 8);
                        for (int i = 0; i < trunkHeight; i++)
                        {
                            Vector3Int treePosition = new Vector3Int(x, chunkPosition.y + y + 1 + i, 0);
                            tilemap.SetTile(treePosition, treeTile);
                        }
                        
                    }
                }
            }
        }
    }

    
}
