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
    bool OnPause;
    Vector2Int DestBlock;
    bool ValidDestBlock;
    Vector2[,] Positions;

    void Start()
    {
        GridModel = new Model();
        GridModel.CreateGrid(Width, Height);
        GridModel.SetGridPositions();
        Blocks = new GameObject[Width, Height];
        Positions = new Vector2[Width, Height];
        Processing = false;
        OnPause = false;
        ValidDestBlock = false;

        while (CheckHorizontalRepeated() ||
        CheckVerticalRepeated()) ;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Blocks[i, j] = GridView.GetNewTile(i, j, Width, Height, GridModel.GetValue(i, j));
            }
        }

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Positions[i, j] = Blocks[i, j].transform.localPosition;
            }
        }

        GridView.AlignGrid(Width, Height);
    }

    private void Update()
    {
        if (!OnPause)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            if (Input.GetMouseButton(0) && !Processing)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit && !IsBlockClicked)
                {
                    hit.transform.GetComponent<SpriteRenderer>().color = Color.gray;
                    GetClickedBlock(hit.transform.gameObject);
                }
                else if(hit)
                {
                    GetDestBlock(hit.transform.gameObject);
                }

            }
            else if (Input.GetMouseButtonUp(0) && !Processing)
            {
                if(ValidDestBlock)
                {
                    GetButtonUpBlock();
                }
                else
                {
                    Blocks[LastBlockPos.x, LastBlockPos.y].GetComponent<SpriteRenderer>().color = Color.white;
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
                            hit.transform.GetComponent<SpriteRenderer>().color = Color.gray;
                            GetClickedBlock(hit.transform.gameObject);
                            break;
                        case TouchPhase.Moved:
                            GetDestBlock(hit.transform.gameObject);
                            break;
                        case TouchPhase.Ended:
                            Blocks[LastBlockPos.x, LastBlockPos.y].GetComponent<SpriteRenderer>().color = Color.white;
                            GetButtonUpBlock();
                            break;
                        case TouchPhase.Canceled:
                            Blocks[LastBlockPos.x, LastBlockPos.y].GetComponent<SpriteRenderer>().color = Color.white;
                            GetButtonUpBlock();
                            break;
                        default:
                            break;
                    }    
                }
                else
                {
                    if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && !ValidDestBlock)
                    {
                        Blocks[LastBlockPos.x, LastBlockPos.y].GetComponent<SpriteRenderer>().color = Color.white;
                        IsBlockClicked = false;
                    }
                    else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled && ValidDestBlock)
                    {
                        Blocks[LastBlockPos.x, LastBlockPos.y].GetComponent<SpriteRenderer>().color = Color.white;
                        GetButtonUpBlock();
                    }
                }
            }
#endif
        }
    }

    public void Pause()
    {
        OnPause = true;
    }

    public void Unpause()
    {
        OnPause = false;
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
                    if (IsValidType(GridModel.GetValue(i, j)))
                    {
                        IsBlockClicked = true;
                        LastBlockPos = new Vector2Int(i, j);
                    }
                }
            }
        }
    }

    void GetDestBlock(GameObject Block)
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
                            DestBlock = new Vector2Int(i, j);
                            ValidDestBlock = true;
                        }
                    }
                }
            }
        }
    }

    public void GetButtonUpBlock()
    {
        if (IsBlockClicked && ValidDestBlock)
        {
            IsBlockClicked = false;
            StartCoroutine(ReplaceValues(DestBlock.x, DestBlock.y, LastBlockPos.x, LastBlockPos.y, 0f));
            CheckHorizontalMatch(DestBlock.y);
            if (DestBlock.y != LastBlockPos.y)
                CheckHorizontalMatch(LastBlockPos.y);
            CheckVerticalMatch(DestBlock.x);
            if (DestBlock.x != LastBlockPos.x)
                CheckVerticalMatch(LastBlockPos.x);
            Debug.Log(MatchesCount);
            if (MatchesCount > 0)
                StartCoroutine(DeleteRepeated());
            else
                StartCoroutine(ReplaceValues(DestBlock.x, DestBlock.y, LastBlockPos.x, LastBlockPos.y, 0.5f));
            ValidDestBlock = false;
        }

        ValidDestBlock = false;
        IsBlockClicked = false;
    }

    void CheckVerticalMatch(int x)
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
                    Score += 10 * (j + 1);
                }
                repeated = 1;
                matchBreak = false;
            }
        }
    }

    void CheckHorizontalMatch(int y)
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
                    Score += 10 * (j + 1);
                }
                repeated = 1;
                matchBreak = false;
            }
        }
    }

    void CheckAllGridMatches()
    {
        for (int i = 0; i < Width; i++)
        {
            CheckVerticalMatch(i);
        }
        for (int i = 0; i < Height; i++)
        {
            CheckHorizontalMatch(i);
        }
        if (MatchesCount > 0)
            StartCoroutine(DeleteRepeated());
    }

    bool IsValidType(int val)
    {
        return val <= GridModel.Types && val >= 0;
    }

    public int GetScore()
    {
        return Score;
    }

    IEnumerator ReplaceValues(int firstX, int firstY, int secondX, int secondY, float delay)
    {
        Processing = true;

        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        if (Blocks[firstX, firstY])
        {
            Blocks[firstX, firstY].transform.localPosition = Positions[secondX, secondY];
        }
        if (Blocks[secondX,secondY])
            Blocks[secondX, secondY].transform.localPosition = Positions[firstX, firstY];

        GameObject auxGo = Blocks[firstX, firstY];
        Blocks[firstX, firstY] = Blocks[secondX, secondY];
        Blocks[secondX, secondY] = auxGo;

        int AuxType = GridModel.GetValue(firstX, firstY);
        GridModel.SetValue(firstX, firstY, GridModel.GetValue(secondX, secondY));
        GridModel.SetValue(secondX, secondY, AuxType);



        //GridView.RefreshSprite(ref Blocks[firstX, firstY], GridModel.GetValue(firstX, firstY));
        //GridView.RefreshSprite(ref Blocks[secondX, secondY], GridModel.GetValue(secondX, secondY));

        //int AuxType = GridModel.GetValue(firstX, firstY);
        //GridModel.SetValue(firstX, firstY, GridModel.GetValue(secondX, secondY));
        //GridModel.SetValue(secondX, secondY, AuxType);
        //GridView.RefreshSprite(ref Blocks[firstX, firstY], GridModel.GetValue(firstX, firstY));
        //GridView.RefreshSprite(ref Blocks[secondX, secondY], GridModel.GetValue(secondX, secondY));
        Processing = false;
    }

    IEnumerator DeleteRepeated()
    {
        Processing = true;
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < MatchesCount; i++)
        {
            Destroy(Blocks[matches[i].x, matches[i].y]);
            Blocks[matches[i].x, matches[i].y] = null;
            //GridModel.SetValue(matches[i].x, matches[i].y, DestroyedTileValue);
            //GridView.RefreshSprite(ref Blocks[matches[i].x, matches[i].y], GridModel.GetValue(matches[i].x, matches[i].y));
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
                    if (Blocks[col, j] == null && Blocks[col, j - 1] != null)
                    {
                        StartCoroutine(ReplaceValues(col, j, col, j - 1, 0f));
                        stillReordering = true;
                    }
                }
            }

            for (int j = 0; j < Height; j++)
            {
                if (Blocks[col, j] == null)
                {
                    GridModel.SetValue(col, j, GridModel.GetRandomValue());
                    Blocks[col, j] = GridView.GetNewTile(col, j, Width, Height, GridModel.GetValue(col, j));
                    Blocks[col, j].transform.localPosition = Positions[col, j];
                    GridView.RefreshSprite(ref Blocks[col, j], GridModel.GetValue(col, j));
                }
            }
        }
        Processing = false;
        CheckAllGridMatches();
    }
}
