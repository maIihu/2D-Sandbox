using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    private Tilemap tilemap;
        
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found on this GameObject!");
            return; 
        }

        TilemapCollider2D tileCol = this.gameObject.AddComponent<TilemapCollider2D>();
        tileCol.usedByComposite = true;

        CompositeCollider2D tileCom = this.gameObject.AddComponent<CompositeCollider2D>();
        
        tilemap.RefreshAllTiles();

        Rigidbody2D rbTilemap = tilemap.GetComponent<Rigidbody2D>();
        rbTilemap.bodyType = RigidbodyType2D.Static;

        this.gameObject.layer = LayerMask.NameToLayer("LayerTilemap");
    }
    

    
}