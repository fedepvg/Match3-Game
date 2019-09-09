using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public GameObject TilePrefab;
    public Sprite[] TileSprite;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void InitTiles(int[,] grid, int width, int height)
    {
        Vector3 CameraMinBounds = CameraUtils.OrthographicBounds().min;
        Vector3 CameraExents = CameraUtils.OrthographicBounds().extents;
        float PrefabWidth = TilePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float PrefabHeight = TilePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;        
        float GridWidth = CameraUtils.OrthographicBounds().size.x;
        float GridHeight = CameraUtils.OrthographicBounds().size.y;
        int cant = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(TilePrefab,new Vector2(CameraMinBounds.x + GridWidth / (width - 1) * i + PrefabWidth,
                                            CameraMinBounds.y + GridHeight / (height - 1) * j + PrefabHeight), Quaternion.identity);
                go.name = "Tile" + cant;
                SpriteRenderer TileRenderer = go.GetComponent<SpriteRenderer>();
                TileRenderer.sprite = TileSprite[grid[i, j]];
                cant++;
            }
        }
    }
}
