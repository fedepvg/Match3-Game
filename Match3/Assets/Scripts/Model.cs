using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model
{
    private int[,] Grid;
    private int Width;
    private int Height;

    public void CreateGrid(int width, int height)
    {
        Grid = new int[width, height];
        Width = width;
        Height = height;
    }

    public void SetGridPositions()
    {
        int type;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                type = Random.Range(0, 4);
                Grid[i, j] = type;
            }
        }
    }

    public int[,] GetGrid()
    {
        return Grid;
    }

    public int GetValue(int posX, int posY)
    {
        return Grid[posX, posY];
    }

    public void SetValue(int posX,int posY, int value)
    {
        Grid[posX, posY] = value;
    }
}