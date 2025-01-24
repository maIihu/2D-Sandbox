using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildController : MonoBehaviour
{
    public LayerMask layer;
    
    private RaycastHit2D hit;
    
    public TileClass treeTile;
    public int chunkSize = 16;
    public List<TilemapInfo> Tilemaps;

    public GameObject tileDrop;
    
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
                    tilemap.SetTile(cellPos, treeTile.tileSprite);
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
                    Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos); // tọa độ lươới
                    TileBase tile = tilemap.GetTile(cellPos);
                    if (tile != null)
                    {
                        if (tile == treeTile.tileSprite)
                        {
                            tilemap.SetTile(cellPos, null);
                            Vector3 worldPos = tilemap.CellToWorld(cellPos) + tilemap.tileAnchor; // chuyển qua tọa độ thế giới
                            GameObject newTileDrop = Instantiate(tileDrop, worldPos, Quaternion.identity);
                            ItemClass itemDrop = new ItemClass(treeTile);
                            newTileDrop.GetComponent<TileDropController>().item = itemDrop;
                        }
                    }
                }
            }
        }
    }
    
}
