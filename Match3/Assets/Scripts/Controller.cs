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
    const int DestroyedTileValue = 5;
    List<Vector2Int> matches = new List<Vector2Int>();
    int MatchesCount = 0;
    bool Processing;

    void Start()
    {
        GridModel = new Model();
        GridModel.CreateGrid(Width, Height);
        GridModel.SetGridPositions();
        Blocks = new GameObject[Width, Height];
        Processing = false;

        while (CheckHorizontalRepeated() ||
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
#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !Processing)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                GetClickedBlock(hit.transform.gameObject);
            }
            
        }
        else if(Input.GetMouseButtonUp(0) && !Processing)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                GetButtonUpBlock(hit.transform.gameObject);
            }
            else
            {
                IsBlockClicked = false;
            }
        }
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount > 0  && !Processing)
        {
            Touch touch = Input.GetTouch(0);

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

            if (hit)
            {
                switch(touch.phase)
                {
                    case TouchPhase.Began:
                        GetClickedBlock(hit.transform.gameObject);
                        break;
                    case TouchPhase.Ended:
                        GetButtonUpBlock(hit.transform.gameObject);
                        break;
                    default:
                        IsBlockClicked = false;
                        break;
                }
                
            }

        }
#endif
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
                        actualValue = Random.Range(0, GridModel.Types);
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
                        actualValue = Random.Range(0, GridModel.Types);
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
                    if(IsValidType(GridModel.GetValue(i, j)))
                    {
                        IsBlockClicked = true;
                        LastBlockPos = new Vector2Int(i, j);
                    }
                }
            }
        }
    }

    public void GetButtonUpBlock(GameObject Block)
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (Block == Blocks[i, j])
                {
                    if (IsBlockClicked && IsValidType(GridModel.GetValue(i, j)))
                    {
                        if (((i == LastBlockPos.x - 1 || i == LastBlockPos.x + 1) && j == LastBlockPos.y) !=
                           ((j == LastBlockPos.y - 1 || j == LastBlockPos.y + 1) && i == LastBlockPos.x))
                        {
                            IsBlockClicked = false;
                            ReplaceValues(i, j, LastBlockPos.x, LastBlockPos.y);
                            CheckHorizontalMatch(i);
                            if(i!=LastBlockPos.x)
                                CheckHorizontalMatch(LastBlockPos.x);
                            CheckVerticalMatch(j);
                            if (j != LastBlockPos.y)
                                CheckVerticalMatch(LastBlockPos.y);
                            if (MatchesCount > 0)
                                StartCoroutine(DeleteRepeated());
                            
                        }
                    }
                }
            }
        }

        IsBlockClicked = false;
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
            if (actualValue > GridModel.Types)
                actualValue = -1;
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
            if (actualValue > GridModel.Types)
                actualValue = -1;
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
        }
    }

    void CheckHorizontalMatch(int x)
    {
        int repeated = 1;
        int lastValue = -1;
        int actualValue;
        bool matchBreak = false;

        for (int i = 0; i < Height; i++)
        {
            actualValue = GridModel.GetValue(x, i);
            if (actualValue > GridModel.Types)
                actualValue = -1;
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
                    MatchesCount++;
                    Score += 10;
                }
                repeated = 1;
                matchBreak = false;
            }
        }
    }

    void CheckVerticalMatch(int y)
    {
        int repeated = 1;
        int lastValue = -1;
        int actualValue;
        bool matchBreak = false;

        for (int i = 0; i < Width; i++)
        {
            actualValue = GridModel.GetValue(i, y);
            if (actualValue > GridModel.Types)
                actualValue = -1;
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
            if (matchBreak)
            {
                for (int j = 0; j < repeated; j++)
                {
                    matches.Add(new Vector2Int(i - j, y));
                    MatchesCount++;
                    Score += 10;
                }
                repeated = 1;
                matchBreak = false;
            }
        }
    }

    void CheckAllGridMatches()
    {

    }

    void ReplaceValues(int firstX, int firstY, int secondX, int secondY)
    {
        int AuxType = GridModel.GetValue(firstX, firstY);
        GridModel.SetValue(firstX, firstY, GridModel.GetValue(secondX, secondY));
        GridModel.SetValue(secondX, secondY, AuxType);
        GridView.RefreshSprite(ref Blocks[firstX, firstY], GridModel.GetValue(firstX, firstY));
        GridView.RefreshSprite(ref Blocks[secondX, secondY], GridModel.GetValue(secondX, secondY));
    }

    bool IsValidType(int val)
    {
        return val <= GridModel.Types && val >= 0;
    }

    IEnumerator DeleteRepeated()
    {
        Processing = true;
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < MatchesCount; i++)
        {
            GridModel.SetValue(matches[i].x, matches[i].y, DestroyedTileValue);
            GridView.RefreshSprite(ref Blocks[matches[i].x, matches[i].y], GridModel.GetValue(matches[i].x, matches[i].y));
        }

        HashSet<int> columnsMatched = new HashSet<int>();
        foreach (Vector2Int vector in matches)
        {
            columnsMatched.Add(vector.x);
        }
        List<int> columnList = new List<int>(columnsMatched);
        StartCoroutine(ReorderGrid(columnList));
        matches.Clear();
        MatchesCount = 0;
    }

    IEnumerator ReorderGrid(List<int> columnList)
    {
        yield return new WaitForSeconds(0.5f);

        foreach (int col in columnList)
        {
            bool stillReordering = true;
            while (stillReordering)
            {
                stillReordering = false;

                for (int j = 1; j < Height; j++)
                {
                    if (GridModel.GetValue(col, j) == DestroyedTileValue && GridModel.GetValue(col, j - 1) != DestroyedTileValue)
                    {
                        ReplaceValues(col, j, col, j - 1);
                        stillReordering = true;
                    }
                }
            }

            for (int j = 0; j < Height; j++)
            {
                if (GridModel.GetValue(col, j) == DestroyedTileValue)
                {
                    GridModel.SetValue(col, j, GridModel.GetRandomValue());
                    GridView.RefreshSprite(ref Blocks[col, j], GridModel.GetValue(col, j));
                }
            }
        }
        Processing = false;
    }
}
