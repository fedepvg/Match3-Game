using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public GameObject TilePrefab;
    public Sprite[] TileSprite;

    public GameObject GetNewTile(int GridXPos, int GridYPos, int GridWidth, int GridHeight, int BlockType)
    {
        float PrefabWidth = TilePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float PrefabHeight = TilePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;        
        GameObject Block = Instantiate(TilePrefab,new Vector2(PrefabWidth * GridXPos, PrefabHeight * GridYPos * -1), Quaternion.identity);
        Block.name = "Tile";
        Block.AddComponent<BoxCollider2D>();
        SpriteRenderer TileRenderer = Block.GetComponent<SpriteRenderer>();
        TileRenderer.sprite = TileSprite[BlockType];

        return Block;
    }

    public void RefreshSprite(ref GameObject Block, int type)
    {
        if (type <= 4 && type >= 0)
            Block.GetComponent<SpriteRenderer>().sprite = TileSprite[type];
        else
        {
            Block.GetComponent<SpriteRenderer>().sprite = TileSprite[0];
            Block.GetComponent<SpriteRenderer>().color = Color.clear;
        }
    }
}
