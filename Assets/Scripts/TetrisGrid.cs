﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    public static int number_of_rows = 20;
    public static int number_of_columns = 10;

    public static int currentHeight = 0;

    //public static Transform[,] grid = new Transform[number_of_columns, number_of_rows];
    public static Transform[,] grid = new Transform[number_of_columns, number_of_rows];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static Vector2 RoundVector(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }

    public static bool IsInsideBorder(Vector2 position)
    {
        return (int)position.x >= 0 && (int)position.x < number_of_columns && (int)position.y >= 0;
    }

    private static void DecreaseRow(int y)
    {
        for (int x = 0; x < number_of_columns; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    private static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < number_of_rows; ++i)
        {
            DecreaseRow(i);
        }
    }

    private static bool IsRowFull(int y)
    {
        for (int x = 0; x < number_of_columns; ++x)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    private static void DeleteRow(int y)
    {
        for (int x = 0; x < number_of_columns; ++x)
        {
            GameObject.Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public static void DeleteRows()
    {
        for (int y = 0; y < number_of_rows; ++y)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                ScoreManager.Instance.AddScore();
                y--;
            }
        }
    }

    public static void UpdateTetrisGrid(Transform tetrisBLock)
    {
        for (int y = 0; y < number_of_rows; ++y)
        {
            for (int x = 0; x < number_of_columns; ++x)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetrisBLock)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform child in tetrisBLock)
        {
            Vector2 vector = RoundVector(child.position);
            grid[(int)vector.x, (int)vector.y] = child;
        }
    }

    public static bool IsValidGridPosition(Transform tetrisBLock)
    {
        foreach (Transform child in tetrisBLock)
        {
            Vector2 vector = RoundVector(child.position);

            if (!TetrisGrid.IsInsideBorder(vector))
            {
                return false;
            }

            if (grid[(int)vector.x, (int)vector.y] != null && grid[(int)vector.x, (int)vector.y].parent != tetrisBLock)
            {
                return false;
            }
        }
        return true;
    }

    private static void FindCurrentHeight()
    {
        for (int y = 0; y < number_of_rows; ++y)
        {
            for (int x = 0; x < number_of_columns; ++x)
            {
                if (grid[x, y] != null)
                {
                    if((y + 1) >= currentHeight)
                    {
                        currentHeight = y + 1;
                    }
                }
            }
        }
    }

    public static bool IsEndGame()
    {
        FindCurrentHeight();
        print(currentHeight);

        if (currentHeight >= number_of_rows - 3)
        {
            return true;
        }
        return false;
    }
}
