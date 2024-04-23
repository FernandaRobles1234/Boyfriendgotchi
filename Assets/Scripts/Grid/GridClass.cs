using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridClass<T>
{
    private int width;
    private int height;
    public float cellSize;
    private T[,] gridArray;
    private Vector3 startPosition;

    // Constructor for the grid
    public GridClass(int width, int height, Vector3 startPosition, float cellSize, System.Func<GridClass<T>, int, int, T> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.startPosition = startPosition;
        gridArray = new T[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }
    }

    public int Width
    {
        get { return width; }
    }

    public int Height
    {
        get { return height; }
    }

    // Method to get the value of a cell
    public T GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(T); // Return default value if the index is out of range
        }
    }

    // Method to set the value of a cell
    public void SetValue(int x, int y, T value)
    {
        if (x >= 0 && y >= 0 && x < width  && y < height)
        {
            gridArray[x, y] = value;
        }
    }

    // Method to get the world position of a cell
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0) * cellSize + startPosition;
    }

    // Method to get the grid coordinates from the world position
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.x - startPosition.x)/ cellSize);
        y = Mathf.FloorToInt((worldPosition.y - startPosition.y)/ cellSize);
    }

    // Method to display the grid in the Unity Editor
    public void DebugDrawGrid()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.red, 1000f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red, 1000f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 1000f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 1000f);

        
    }
}
