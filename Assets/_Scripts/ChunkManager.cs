using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    public int chunkSize = 16;              // Kích thước mỗi chunk
    public int viewDistance = 3;            // Khoảng cách tầm nhìn
    public Transform player;                // Người chơi hoặc camera
    public MapRenderer mapRenderer;         // Tham chiếu đến MapRenderer
    private Vector3Int currentChunkPosition;
    private Dictionary<Vector3Int, GameObject> chunkObjects = new Dictionary<Vector3Int, GameObject>(); // Lưu các GameObject chunk

    void Start()
    {
        currentChunkPosition = Vector3Int.zero; // Vị trí bắt đầu của người chơi
        GenerateMap(currentChunkPosition);      // Tạo map ban đầu
    }

    void Update()
    {
        // Cập nhật vị trí chunk mỗi khi người chơi di chuyển
        Vector3Int playerChunkPosition = GetPlayerChunkPosition();
        if (playerChunkPosition != currentChunkPosition)
        {
            currentChunkPosition = playerChunkPosition;
            GenerateMap(currentChunkPosition);  // Tạo lại các chunk xung quanh người chơi
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
        // Tạo và ẩn các chunk trong phạm vi nhìn thấy
        List<Vector3Int> chunksToRemove = new List<Vector3Int>();

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int y = -viewDistance; y <= viewDistance; y++)
            {
                Vector3Int chunkPosition = new Vector3Int(centerChunk.x + x * chunkSize, centerChunk.y + y * chunkSize, 0);

                if (!chunkObjects.ContainsKey(chunkPosition))
                {
                    GenerateChunk(chunkPosition);
                }
                else
                {
                    // Giữ lại chunk đã hiện
                    chunkObjects[chunkPosition].SetActive(true);
                }
            }
        }

        // Xóa các chunk xa khỏi phạm vi tầm nhìn
        foreach (var chunk in chunkObjects)
        {
            Vector3Int chunkPosition = chunk.Key;

            if (Vector3Int.Distance(centerChunk, chunkPosition) > viewDistance * chunkSize)
            {
                chunk.Value.SetActive(false);  // Tắt chunk nếu ra ngoài tầm nhìn
            }
        }
    }

    void GenerateChunk(Vector3Int chunkPosition)
    {
        GameObject chunkObject = new GameObject("Chunk_" + chunkPosition);
        chunkObject.SetActive(true); // Bật chunk khi tạo
        chunkObjects[chunkPosition] = chunkObject;

        // Tạo và gắn các tile vào chunk
        mapRenderer.RenderChunk(chunkPosition); // Lệnh vẽ chunk
    }
}
