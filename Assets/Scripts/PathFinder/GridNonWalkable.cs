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
    private GridClass _grid;
    private int _newX;
    private int _newY;

    public PathNode _nonWalkableValue;

    void Start()
    {
        _pathFinding = _goPathFinding.GetComponent<InitPathFinding>()._pathFinding;
        _grid = _pathFinding._grid;

        // Calculate the world position of the box collider bounds
        Vector3 worldLowerLeft = _boxCollider.bounds.min;
        Vector3 worldUpperRight = _boxCollider.bounds.max;

        // Get grid coordinates for the lower-left and upper-right corners
        _grid.GetXY(worldLowerLeft, out _newX, out _newY);
        _grid.GetXY(worldUpperRight, out int endX, out int endY);

        // Iterate over the grid cells covered by the box collider
        for (int xIndex = _newX; xIndex <= endX; xIndex++)
        {
            for (int yIndex = _newY; yIndex <= endY; yIndex++)
            {
                if (xIndex < _grid.Width && yIndex < _grid.Height)
                {
                    PathNode cell = _grid.GetValue(xIndex, yIndex);
                    if (cell != null)
                    {
                        cell._isWalkable = false;
                    }
                }
            }
        }
    }
}