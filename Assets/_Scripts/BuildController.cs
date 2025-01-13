using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildController : MonoBehaviour
{
    public LayerMask layer;
    
    private RaycastHit2D hit;
    
    public TileBase treeTile;
    public int chunkSize = 16;
    public List<TilemapInfo> Tilemaps;
    
    void Start()
    {
        ChunkManager chunkManager = FindObjectOfType<ChunkManager>();
        Tilemaps = chunkManager.GetTilemaps();
    }
    
    void Update()
    {
        DestroyBlock();
        BuildBlock();
    }

    void BuildBlock()
    {
        if (Input.GetMouseButton(1)) 
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            int posX = Mathf.FloorToInt(mouseWorldPos.x);
            TilemapInfo targetTilemap = Tilemaps.Find(x => posX >= x.PosX && posX < x.PosX + chunkSize);
            if (targetTilemap != null)
            {
                Tilemap tilemap = targetTilemap.Tilemap;
                Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);
            
                if (tilemap.GetTile(cellPos) == null)
                {
                    tilemap.SetTile(cellPos, treeTile);
                }
                
            }
        }
    }

    void DestroyBlock()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            hit = Physics2D.Raycast(mouseWorldPos, Vector2.up, Mathf.Infinity, layer.value);
            if (hit.collider)
            {
                Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
                if (tilemap)
                {
                    Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);
                    TileBase tile = tilemap.GetTile(cellPos);
                    if (tile != null)
                    {
                        if (tile.name == "tree")
                        {
                            tilemap.SetTile(cellPos, null);
                        }
                    }
                }
            }
        }
    }
    
}
