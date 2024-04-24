using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public GridClass<PathNode> _grid;
    public int _x;
    public int _y;

    public int _gCost;
    public int _hCost;
    public int _fCost;

    public bool _isWalkable;
    public PathNode _parentNode;
    public PathNode(GridClass<PathNode> grid, int x, int y)
    {
        _grid = grid;
        _x = x;
        _y = y;
        _isWalkable = true;
    }

    public void CalculateFCost()
    {
        _fCost = _gCost + _fCost;
    }
    public override string ToString()
    {
        return _x + ", " + _y + "," + _isWalkable.ToString();
    }
}
