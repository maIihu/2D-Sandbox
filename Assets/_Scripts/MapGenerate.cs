using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ChunkManager1 : MonoBehaviour
{
    public Tilemap tilemap;               // Tilemap chính để chứa các chunk
    public TileBase groundTile;           // Tile đất
    public TileBase waterTile;            // Tile nước
    public int chunkSize = 16;            // Kích thước mỗi chunk (ví dụ: 16x16 tiles)
    public int viewDistance = 3;          // Khoảng cách số chunk có thể nhìn thấy xung quanh người chơi
    public Transform player;              // Người chơi hoặc camera

    private Vector3Int currentChunkPosition;
    private Dictionary<Vector3Int, Tilemap> chunkMap = new Dictionary<Vector3Int, Tilemap>(); // Lưu trữ các chunk đã tạo

    void Start()
    {
        currentChunkPosition = Vector3Int.zero; // Vị trí bắt đầu của người chơi, có thể thay bằng vị trí camera hoặc người chơi
        GenerateMap(currentChunkPosition);
    }

    void Update()
    {
        // Cập nhật vị trí chunk mỗi khi người chơi di chuyển
        Vector3Int playerChunkPosition = GetPlayerChunkPosition();
        if (playerChunkPosition != currentChunkPosition)
        {
            currentChunkPosition = playerChunkPosition;
            GenerateMap(currentChunkPosition);
        }
    }

    Vector3Int GetPlayerChunkPosition()
    {
        // Tính toán chunk vị trí của người chơi, chia theo kích thước chunkSize
        return new Vector3Int(
            Mathf.FloorToInt(player.position.x / chunkSize) * chunkSize,
            Mathf.FloorToInt(player.position.y / chunkSize) * chunkSize,
            0
        );
    }

    void GenerateMap(Vector3Int centerChunk)
    {
        // Xóa các chunk không còn trong phạm vi nhìn thấy của người chơi
        UnloadFarChunks(centerChunk);

        // Tạo các chunk mới trong phạm vi nhìn thấy
        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int y = -viewDistance; y <= viewDistance; y++)
            {
                Vector3Int chunkPosition = new Vector3Int(centerChunk.x + x * chunkSize, centerChunk.y + y * chunkSize, 0);
                if (!chunkMap.ContainsKey(chunkPosition))
                {
                    GenerateChunk(chunkPosition);
                }
            }
        }
    }

    void GenerateChunk(Vector3Int chunkPosition)
    {
        // Tạo Tilemap cho chunk mới
        Tilemap newChunk = new GameObject("Chunk_" + chunkPosition).AddComponent<Tilemap>();
        TilemapRenderer renderer = newChunk.gameObject.AddComponent<TilemapRenderer>();
        newChunk.transform.parent = tilemap.transform; // Gắn chunk vào Tilemap cha

        chunkMap[chunkPosition] = newChunk; // Lưu trữ chunk

        // Vẽ tiles cho chunk này
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                float xCoord = (float)(chunkPosition.x + x) / 100f; // Điều chỉnh tỉ lệ Perlin Noise
                float yCoord = (float)(chunkPosition.y + y) / 100f;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                TileBase tile = sample < 0.4f ? waterTile : groundTile;
                newChunk.SetTile(new Vector3Int(chunkPosition.x + x, chunkPosition.y + y, 0), tile);
            }
        }
    }

    void UnloadFarChunks(Vector3Int centerChunk)
    {
        List<Vector3Int> chunksToUnload = new List<Vector3Int>();

        foreach (var chunk in chunkMap)
        {
            Vector3Int chunkPosition = chunk.Key;

            // Kiểm tra xem chunk này có nằm trong phạm vi cần tải lại không
            if (Vector3Int.Distance(centerChunk, chunkPosition) > viewDistance * chunkSize)
            {
                chunksToUnload.Add(chunkPosition);
            }
        }

        // Hủy các chunk xa không cần thiết
        foreach (Vector3Int chunkPosition in chunksToUnload)
        {
            Destroy(chunkMap[chunkPosition].gameObject); // Hủy Tilemap chunk
            chunkMap.Remove(chunkPosition); // Xóa khỏi danh sách chunk
        }
    }
}
