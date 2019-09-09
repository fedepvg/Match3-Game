using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public GameObject TilePrefab;
    public Sprite[] TileSprite;
    public GameObject[,] Blocks;

    public void InitTiles(int[,] grid, int width, int height)
    {
        Vector3 CameraMinBounds = CameraUtils.OrthographicBounds().min;
        Vector3 CameraExents = CameraUtils.OrthographicBounds().extents;
        float PrefabWidth = TilePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float PrefabHeight = TilePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;        
        float GridWidth = CameraUtils.OrthographicBounds().size.x;
        float GridHeight = CameraUtils.OrthographicBounds().size.y;
        int cant = 0;

        Blocks = new GameObject[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //GameObject go = Instantiate(TilePrefab,new Vector2(CameraMinBounds.x + GridWidth / width * i + PrefabWidth,
                //                            CameraMinBounds.y + GridHeight / height * j + PrefabHeight), Quaternion.identity);
                Blocks[i,j]= Instantiate(TilePrefab, new Vector2(CameraMinBounds.x + GridWidth / width * i + PrefabWidth,
                                            CameraMinBounds.y + GridHeight / height * j + PrefabHeight), Quaternion.identity);
                Blocks[i, j].name = "Tile" + cant;
                Blocks[i, j].AddComponent<BoxCollider2D>();
                SpriteRenderer TileRenderer = Blocks[i, j].GetComponent<SpriteRenderer>();
                TileRenderer.sprite = TileSprite[grid[i, j]];
                cant++;
            }
        }
    }
}
