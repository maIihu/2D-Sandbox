using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    public int chunkSize = 16;             
    public Transform player;              
    public MapRenderer mapRenderer;   
    
    private Vector3Int currentChunkPosition;
    private Dictionary<Vector3Int, GameObject> chunkObjects = new Dictionary<Vector3Int, GameObject>();
    
    private List<TilemapInfo> tilemaps = new List<TilemapInfo>();
    
    void Start()
    {
        currentChunkPosition = Vector3Int.zero; 
        GenerateMap(currentChunkPosition);     
    }

    void Update()
    {
        Vector3Int playerChunkPosition = GetPlayerChunkPosition();
        if (playerChunkPosition != currentChunkPosition)
        {
            currentChunkPosition = playerChunkPosition;
            GenerateMap(currentChunkPosition);  
        }
    }

    Vector3Int GetPlayerChunkPosition()
    {
        return new Vector3Int(
            Mathf.FloorToInt(player.position.x / chunkSize) * chunkSize,
            Mathf.FloorToInt(player.position.y / chunkSize) * chunkSize,
            0
        );
    }

    void GenerateMap(Vector3Int playerPos)
    {
        int width = 10;  

        for (int x = -width / 2; x < width / 2; x++)
        {
            Vector3Int chunkPosition = new Vector3Int(playerPos.x + x * chunkSize, 0, 0);

            if (!chunkObjects.ContainsKey(chunkPosition))
            {
                GenerateChunk(chunkPosition);
            }
            else
            {
                if(chunkObjects.ContainsKey(chunkPosition))
                    chunkObjects[chunkPosition].SetActive(true);
            }
        }

        foreach (var chunk in chunkObjects)
        {
            Vector3Int chunkPosition = chunk.Key;

            if (Mathf.Abs(chunkPosition.x - playerPos.x) > width * chunkSize / 2 )
            {
                chunk.Value.SetActive(false);  
            }
        }
    }
    
    void GenerateChunk(Vector3Int chunkPosition)
    {
        GameObject gridObject = new GameObject("Grid_" + chunkPosition);
        gridObject.transform.SetParent(this.transform);
        gridObject.AddComponent<Grid>();
        gridObject.transform.position = new Vector3(chunkPosition.x, chunkPosition.y, 0);
        gridObject.layer = LayerMask.NameToLayer("LayerTilemap");
        
        GameObject tilemapObject = new GameObject("Tilemap_" + chunkPosition);
        tilemapObject.transform.parent = gridObject.transform;
        
        Tilemap tilemap = tilemapObject.AddComponent<Tilemap>();
        tilemapObject.AddComponent<TilemapRenderer>();
        
        
        TilemapController tilemapController = tilemapObject.AddComponent<TilemapController>();
        
        chunkObjects[chunkPosition] = gridObject;
        
        mapRenderer.RenderChunk(chunkPosition, tilemap);
        //tilemapObject.transform.position = new Vector3(chunkPosition.x, chunkPosition.y, 0);
        
        tilemaps.Add(new TilemapInfo(tilemap, chunkPosition.x));
    }
    
    public List<TilemapInfo> GetTilemaps()
    {
        return tilemaps; 
    }
    
}
