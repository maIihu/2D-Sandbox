using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGenerate : MonoBehaviour
{
    public int width = 100;
    public int height = 50;
    
    public float terrainScale = 20f;
    public float caveScale = 10f;
    public float caveThreshold = 0.5f;

    public Tilemap tileMap;
    public TileBase dirtTile;
    public TileBase dirtSurTile;
    public TileBase emptyTile;
    public TileBase treeTile;
    
    private void Start()
    {
        GenerateMap();
        GenerateTree();
    }

    private void GenerateMap()
    {
        float seed = Random.Range(0f, 100f);
        for (int x = 0; x < width; x++)
        {
            int groundHeight = Mathf.FloorToInt(Mathf.PerlinNoise(x / terrainScale, seed) * height / 2 + height / 4);
            for (int y = 0; y <= groundHeight; y++)
            {
                float caveNoise = Mathf.PerlinNoise(x / caveScale + seed, y / caveScale + seed);
                if (y == 0)
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), dirtTile);
                }
                else if (caveNoise < caveThreshold)
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), emptyTile);
                }
                else if (y == groundHeight)
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), dirtSurTile);
                }
                else
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), dirtTile);
                }
            }
        }
    }

    private void GenerateTree()
    {
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tileMap.GetTile(new Vector3Int(x, y, 0)) == dirtSurTile)
                {
                    if (Random.Range(0f, 1f) < 0.1f)
                    {
                        GrowTree(x, y + 1);
                    }
                }
            }
        }
    }

    private void GrowTree(int x, int y)
    {
        int trunkHeight = Random.Range(3, 8);
        for (int i = 0; i < trunkHeight; i++)
        {
            tileMap.SetTile(new Vector3Int(x, y + i, 0), treeTile);
        }
    }
}

