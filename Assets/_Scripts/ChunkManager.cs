using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    public int chunkSize = 16;             
    public int viewDistance = 2;          
    public Transform player;              
    public MapRenderer mapRenderer;       
    private Vector3Int currentChunkPosition;
    private Dictionary<Vector3Int, GameObject> chunkObjects = new Dictionary<Vector3Int, GameObject>();
    public float limit_y = 100;

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
        int depth = 5;  

        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                // Chỉ tạo chunk ở bên dưới nhân vật
                Vector3Int chunkPosition = new Vector3Int(playerPos.x + x * chunkSize, playerPos.y - y * chunkSize, 0);

                if (chunkPosition.y <= 100 - 16 && !chunkObjects.ContainsKey(chunkPosition))
                {
                    GenerateChunk(chunkPosition);
                }
                else
                {
                    if(chunkObjects.ContainsKey(chunkPosition))
                        chunkObjects[chunkPosition].SetActive(true);
                }
            }
        }

        foreach (var chunk in chunkObjects)
        {
            Vector3Int chunkPosition = chunk.Key;

            if (Mathf.Abs(chunkPosition.x - playerPos.x) > width * chunkSize / 2 ||
                chunkPosition.y > playerPos.y || 
                chunkPosition.y < playerPos.y - depth * chunkSize)
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
        
        GameObject tilemapObject = new GameObject("Tilemap_" + chunkPosition);
        tilemapObject.transform.parent = gridObject.transform;
        
        Tilemap tilemap = tilemapObject.AddComponent<Tilemap>();
        tilemapObject.AddComponent<TilemapRenderer>();
        
        chunkObjects[chunkPosition] = gridObject;
        
        mapRenderer.RenderChunk(chunkPosition, tilemap);
        //tilemapObject.transform.position = new Vector3(chunkPosition.x, chunkPosition.y, 0);
    }




}
