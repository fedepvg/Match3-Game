using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Model GridModel;
    public View GridView;
    public int Width;
    public int Height;
    GameObject[,] Blocks;
    Vector2Int SelectedPosition;
    bool IsBlockClicked;

    void Start()
    {
        GridModel = new Model();
        GridModel.CreateGrid(Width, Height);
        GridModel.SetGridPositions();
        Blocks = new GameObject[Width, Height];

        for (int i = 0; i < Height; i++)
        {
            CheckHorizontalRepeated();
            CheckVerticalRepeated();
        }

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Blocks[i,j] = GridView.GetNewTile(i, j, Width, Height, GridModel.GetValue(i,j));
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                GetClickedBlock(hit.transform.gameObject);
            }
            
        }
    }

    void CheckVerticalRepeated()
    {
        int repeated = 1;
        int lastValue = -1;
        int actualValue;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                actualValue = GridModel.GetValue(i, j);
                if (actualValue == lastValue)
                    repeated++;
                if (repeated >= 3)
                {
                    while (actualValue == lastValue)
                    {
                        actualValue = Random.Range(0, 4);
                    }
                    GridModel.SetValue(i, j, actualValue);
                    repeated = 1;
                }

                lastValue = actualValue;
            }
        }
    }

    void CheckHorizontalRepeated()
    {
        int repeated = 1;
        int lastValue = -1;
        int actualValue;

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                actualValue = GridModel.GetValue(j, i);
                if (actualValue == lastValue)
                    repeated++;
                if (repeated >= 3)
                {
                    while (actualValue == lastValue)
                    {
                        actualValue = Random.Range(0, 4);
                    }
                    GridModel.SetValue(j, i, actualValue);
                    repeated = 1;
                }

                lastValue = actualValue;
            }
        }
    }

    public void GetClickedBlock(GameObject Block)
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (Block == Blocks[i, j])
                {
                    if(IsBlockClicked)
                    {
                        if(i==SelectedPosition.x - 1 || i == SelectedPosition.x + 1||
                           j == SelectedPosition.y - 1 || j == SelectedPosition.y + 1)
                        {
                            IsBlockClicked = false;
                            GameObject AuxGO = Block;
                            Block = Blocks[SelectedPosition.x, SelectedPosition.y];
                            Blocks[SelectedPosition.x, SelectedPosition.y] = AuxGO;
                            Debug.Log("swichea");
                        }
                    }
                    else
                    {
                        IsBlockClicked = true;
                        SelectedPosition = new Vector2Int(i, j);
                    }
                }
            }
        }
    }
}
