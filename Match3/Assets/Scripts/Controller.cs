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
    Vector2Int LastBlockPos;
    bool IsBlockClicked;
    int Score = 0;

    void Start()
    {
        GridModel = new Model();
        GridModel.CreateGrid(Width, Height);
        GridModel.SetGridPositions();
        Blocks = new GameObject[Width, Height];

        while (CheckHorizontalRepeated() &&
        CheckVerticalRepeated()) ;

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

    bool CheckVerticalRepeated()
    {
        int repeated = 1;
        int lastValue = -1;
        int actualValue;
        bool repeats = false;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                actualValue = GridModel.GetValue(i, j);
                if (actualValue == lastValue)
                    repeated++;
                if (repeated >= 3)
                {
                    repeats = true;
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
        return repeats;
    }

    bool CheckHorizontalRepeated()
    {
        int repeated = 1;
        int lastValue = -1;
        int actualValue;
        bool repeats = false;

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                actualValue = GridModel.GetValue(j, i);
                if (actualValue == lastValue)
                    repeated++;
                if (repeated >= 3)
                {
                    repeats = true;
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
        return repeats;
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
                        if(((i == LastBlockPos.x - 1 || i == LastBlockPos.x + 1) && j==LastBlockPos.y) !=
                           ((j == LastBlockPos.y - 1 || j == LastBlockPos.y + 1) && i==LastBlockPos.x))
                        {
                            IsBlockClicked = false;
                            int AuxType = GridModel.GetValue(i, j);
                            GridModel.SetValue(i, j, GridModel.GetValue(LastBlockPos.x, LastBlockPos.y));
                            GridModel.SetValue(LastBlockPos.x, LastBlockPos.y, AuxType);
                            GridView.RefreshSprite(ref Block, GridModel.GetValue(i, j));
                            GridView.RefreshSprite(ref Blocks[LastBlockPos.x,LastBlockPos.y], GridModel.GetValue(LastBlockPos.x,LastBlockPos.y));
                            CheckMatch(i,j);
                            CheckMatch(LastBlockPos.x, LastBlockPos.y);
                        }
                    }
                    else
                    {
                        IsBlockClicked = true;
                        LastBlockPos = new Vector2Int(i, j);
                    }
                }
            }
        }
    }

    void CheckMatch(int x, int y)
    {
        int repeated = 1;
        int lastValue = -1;
        int actualValue;
        bool matchBreak = false;
        List<Vector2Int> matches = new List<Vector2Int>();
        int amountOfMatches = 0;

        for (int i = 0; i < Width; i++)
        {
            actualValue = GridModel.GetValue(i, y);
            if (actualValue == lastValue)
            {
                repeated++;
                if (i == Width - 1 && repeated >= 3)
                    matchBreak = true;
                else
                    matchBreak = false;
            }
            else if (repeated < 3)
            {
                repeated = 1;
                lastValue = actualValue;
            }
            else
            {
                matchBreak = true;
                i--;
            }
            if(matchBreak)
            {
                for(int j = 0; j < repeated; j++)
                {
                    matches.Add(new Vector2Int(i - j, y));
                    amountOfMatches++;
                    Score += 10;
                }
                repeated = 1;
                matchBreak = false;
            }
        }

        lastValue = -1;
        repeated = 1;

        for (int i = 0; i < Height; i++)
        {
            actualValue = GridModel.GetValue(x, i);
            if (actualValue == lastValue)
            {
                repeated++;
                if (i == Height - 1 && repeated >= 3)
                    matchBreak = true;
                else
                    matchBreak = false;
            }
            else if (repeated < 3)
            {
                repeated = 1;
                lastValue = actualValue;
            }
            else
            {
                matchBreak = true;
                i--;
            }
            if (matchBreak)
            {
                for (int j = 0; j < repeated; j++)
                {
                    matches.Add(new Vector2Int(x, i - j));
                    amountOfMatches++;
                    Score += 10;
                }
                repeated = 1;
                matchBreak = false;
            }

            DeleteRepeated(matches, amountOfMatches);
        }
    }

    void DeleteRepeated(List<Vector2Int> matchesList, int matches)
    {
        for(int i = 0; i < matches; i++)
        {
            Destroy(Blocks[matchesList[i].x, matchesList[i].y],1);
        }
    }
}
