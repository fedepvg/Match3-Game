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

    void InitTiles(int[,] grid, int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(TilePrefab);
            }
        }
    }
}
