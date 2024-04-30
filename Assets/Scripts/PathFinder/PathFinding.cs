using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PathFinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_CONST = 14;

    public GridClass _grid;

    private List<PathNode> _openList;
    private List<PathNode> _closedList;
    public PathFinding(int width, int height, Vector3 origin)
    {
        _grid = new GridClass(width, height, origin, 1.0f, (GridClass g, int x, int y) => new PathNode(g, x, y));
    }

    public List<PathNode> FindPath(float startX, float startY, float endX, float endY)
    {
        int startXInt, startYInt, endXInt, endYInt;

        _grid.GetXY(new Vector3(startX, startY, 0), out startXInt, out startYInt);
        _grid.GetXY(new Vector3(endX, endY, 0), out endXInt, out endYInt);

        return FindPath(startXInt, startYInt, endXInt, endYInt);
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = _grid.GetValue(startX, startY);
        PathNode endNode = _grid.GetValue(endX, endY);

        _openList = new List<PathNode>() {startNode};
        _closedList = new List<PathNode>();

        for(int i=0; i < _grid.Width; i++)
        {
            for (int j = 0; j < _grid.Height; j++)
            {
                PathNode pathNode= _grid.GetValue(i, j);
                pathNode._gCost = int.MaxValue;

                pathNode.CalculateFCost();

                pathNode._parentNode = null;
            }
        }

        startNode._gCost = 0;
        startNode._hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (_openList.Count > 0)
        {
            
            PathNode currentNode = GetLowestFCountNode(_openList);

            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (_closedList.Contains(neighbourNode)) continue;

                if (!neighbourNode._isWalkable)
                {
                    _closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode._gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode._gCost)
                {
                    neighbourNode._parentNode = currentNode;
                    neighbourNode._gCost= tentativeGCost;
                    neighbourNode._hCost= CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!_openList.Contains(neighbourNode) )
                    {
                        _openList.Add(neighbourNode);
                    }
                }
            }
        }

        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        // Check left side
        if (currentNode._x - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode._x - 1, currentNode._y)); // Left
            if (currentNode._y - 1 >= 0) // Bottom-left
                neighbourList.Add(GetNode(currentNode._x - 1, currentNode._y - 1));
            if (currentNode._y + 1 < _grid.Height) // Top-left
                neighbourList.Add(GetNode(currentNode._x - 1, currentNode._y + 1));
        }

        // Check right side
        if (currentNode._x + 1 < _grid.Width)
        {
            neighbourList.Add(GetNode(currentNode._x + 1, currentNode._y)); // Right
            if (currentNode._y - 1 >= 0) // Bottom-right
                neighbourList.Add(GetNode(currentNode._x + 1, currentNode._y - 1));
            if (currentNode._y + 1 < _grid.Height) // Top-right
                neighbourList.Add(GetNode(currentNode._x + 1, currentNode._y + 1));
        }

        // Check directly above and below
        if (currentNode._y - 1 >= 0) // Directly below
            neighbourList.Add(GetNode(currentNode._x, currentNode._y - 1));
        if (currentNode._y + 1 < _grid.Height) // Directly above
            neighbourList.Add(GetNode(currentNode._x, currentNode._y + 1));

        return neighbourList;
    }

    private PathNode GetNode(int x, int y)
    {
        return _grid.GetValue(x, y);
    }
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path= new List<PathNode>();
        path.Add(endNode);

        PathNode currentNode = endNode;
        while (currentNode._parentNode != null)
        {
            path.Add(currentNode._parentNode);
            currentNode = currentNode._parentNode;
        }
        path.Reverse();

        return path;
    }
    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a._x - b._x);
        int yDistance = Mathf.Abs(a._y - b._y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_CONST* Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST* remaining;
    }

    private PathNode GetLowestFCountNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];

        for (int i= 1; i< pathNodeList.Count; i++)
        {
            if (pathNodeList[i]._fCost < lowestFCostNode._fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;

    }

    public Vector3 RandomWorldPositionWithin(float centerX, float centerY, Vector2 within)
    {
        Vector3 randomPos;
        int randomX;
        int randomY;

        do
        {
            randomPos = _grid.RandomWorldPositionWithin(centerX, centerY, within);
            _grid.GetXY(randomPos, out randomX, out randomY);
        }
        // Check if the calculated grid coordinates are valid within the grid bounds
        while (_grid.GetValue(randomX, randomY)._isWalkable == false);

        return randomPos;
    }
}
