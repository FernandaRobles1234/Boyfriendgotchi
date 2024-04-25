using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class GridNonWalkable : MonoBehaviour
{
    public GameObject _goPathFinding;
    public BoxCollider2D _boxCollider;

    private PathFinding _pathFinding;
    private GridClass<PathNode> _grid;
    private int _newX;
    private int _newY;

    public PathNode _nonWalkableValue;

    void Start()
    {
        _pathFinding = _goPathFinding.GetComponent<InitPathFinding>()._pathFinding;
        _grid = _pathFinding._grid;

        Vector3 boundsInMeters = _boxCollider.bounds.size;


        Vector3 startPosition = transform.position - (boundsInMeters * 0.5f);
        _grid.GetXY(startPosition, out _newX, out _newY);

        int columns = Mathf.CeilToInt(boundsInMeters.x / _grid.cellSize);
        int rows = Mathf.CeilToInt(boundsInMeters.y / _grid.cellSize);

        Vector3 endPosition = transform.position + (boundsInMeters * 0.5f);
        _grid.GetXY(endPosition, out int endX, out int endY);

        int requiredColumns = (endX - _newX) + 1;
        int requiredRows = (endY - _newY) + 1;

        columns = Mathf.Max(columns, requiredColumns);
        rows = Mathf.Max(rows, requiredRows);

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                int xIndex = _newX + i;
                int yIndex = _newY + j;

                if (xIndex < _grid.Width && yIndex < _grid.Height)
                {
                    PathNode cell = _grid.GetValue(xIndex, yIndex);
                    if (cell != null)
                    {
                        cell._isWalkable = false;
                    }
                    //Debug.Log($"val= {cell}");
                }
            }
        }
        //print("end");
    }

}
