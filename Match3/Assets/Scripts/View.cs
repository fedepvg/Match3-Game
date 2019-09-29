using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public GameObject TilePrefab;
    public Sprite[] TileSprite;
    GameObject Grid;
    float PrefabWidth;
    float PrefabHeight;

    private void Start()
    {
        Grid = new GameObject();
        Grid.transform.position = Vector3.zero;
        Grid.name = "Grid";
        PrefabWidth = TilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        PrefabHeight = TilePrefab.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    public GameObject GetNewTile(int GridXPos, int GridYPos, int GridWidth, int GridHeight, int BlockType)
    {
        GameObject Block = Instantiate(TilePrefab, Grid.transform);
        Block.name = GridXPos + " , " + GridYPos;
        Block.AddComponent<BoxCollider2D>();
        SpriteRenderer TileRenderer = Block.GetComponent<SpriteRenderer>();
        TileRenderer.sprite = TileSprite[BlockType];
        Block.transform.position = new Vector2(PrefabWidth * GridXPos + PrefabWidth * 0.25f * GridXPos, PrefabHeight * GridYPos * -1 - PrefabHeight * 0.25f * GridYPos);

        return Block;
    }

    public void AlignGrid(int GridColumns, int GridRows)
    {
        float GridWidth = GridColumns * PrefabWidth;
        float GridHeight = GridRows * PrefabHeight;
        Grid.transform.position = new Vector2(-GridWidth / 2 + PrefabWidth / 2, GridHeight / 2 + PrefabHeight / 2); 
    }

    public void RefreshSprite(ref GameObject Block, int type)
    {
        if (type <= 4 && type >= 0)
        {
            Block.GetComponent<SpriteRenderer>().sprite = TileSprite[type];
            Block.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            Block.GetComponent<SpriteRenderer>().sprite = TileSprite[0];
            Block.GetComponent<SpriteRenderer>().color = Color.clear;
        }
    }
}
