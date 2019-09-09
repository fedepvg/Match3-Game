using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Model GridModel;
    public View GridView;
    public int Width;
    public int Height;

    void Start()
    {
        GridModel = new Model();
        GridModel.CreateGrid(Width, Height);
        GridModel.SetGridPositions();
        for (int i = 0; i < Height; i++)
        {
            CheckHorizontalRepeated();
            CheckVerticalRepeated();
        }
        GridView.InitTiles(GridModel.GetGrid(), Width, Height);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if(hit)
                Debug.Log("Target: " + hit.transform.name);
            
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
}
