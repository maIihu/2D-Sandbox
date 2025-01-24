using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapRenderer : MonoBehaviour
{

    public TileSet tileSet;
    
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
                    tile = tileSet.dirtTile.tileSprite;
                else if (noiseValue < caveThreshold)
                    tile = tileSet.emptyTile.tileSprite;
                else if (y == groundHeight)
                    tile = tileSet.dirtGrassTile.tileSprite;
                else
                    tile = tileSet.dirtTile.tileSprite;
            
                Vector3Int tilePosition = new Vector3Int(x, chunkPosition.y + y, 0);
                tilemap.SetTile(tilePosition, tile);
                
                if (y == groundHeight && tile == tileSet.dirtGrassTile.tileSprite)
                {
                    float treeChance = Random.value;
                    if (treeChance < treeProbability)
                    {
                        int trunkHeight = Random.Range(4, 6);
                        // Thân cây
                        for (int i = 0; i < trunkHeight; i++)
                        {
                            Vector3Int treePosition = new Vector3Int(x, chunkPosition.y + y + 1 + i, 0);
                            tilemap.SetTile(treePosition, tileSet.trunkTile.tileSprite);
                        }
                        // Lá cây
                        for (int j = trunkHeight; j < 3 + trunkHeight; j++)
                        {
                            Vector3Int leavePos = new Vector3Int(x, chunkPosition.y + y + j, 0);
                            tilemap.SetTile(leavePos, tileSet.leaveTile.tileSprite);
                        }
                        // Tán cây trái
                        tilemap.SetTile(new Vector3Int(x-1, chunkPosition.y + y + trunkHeight, 0), tileSet.leaveTile.tileSprite);
                        tilemap.SetTile(new Vector3Int(x-1, chunkPosition.y + y + trunkHeight+1, 0), tileSet.leaveTile.tileSprite);
                        // Tán cây phải
                        tilemap.SetTile(new Vector3Int(x+1, chunkPosition.y + y + trunkHeight, 0), tileSet.leaveTile.tileSprite);
                        tilemap.SetTile(new Vector3Int(x+1, chunkPosition.y + y + trunkHeight+1, 0), tileSet.leaveTile.tileSprite);
                        
                    }
                }
            }
        }
    }

    private void RenderChunk()
    {
        
    }
    
}
