using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PathFinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_CONST = 14;

    public GridClass<PathNode> _grid;

    private List<PathNode> _openList;
    private List<PathNode> _closedList;
    public PathFinding(int width, int height, Vector3 origin)
    {
        _grid = new GridClass<PathNode>(width, height, origin, 1.0f, (GridClass<PathNode> g, int x, int y) => new PathNode(g, x, y));
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

        int boxWidth = 1;
        int boxHeight = 2;

        int boxOffsetY = -2;
        // Define movements with collision box consideration
        int[] dx = {0, 1, 0, -1}; // Right, Down, Left, Up
        int[] dy = {1, 0, -1, 0};

        for (int direction = 0; direction < 4; direction++)
        {
            bool allWalkable = true;
            for (int offsetX = 0; offsetX < boxWidth; offsetX++)
            {
                for (int offsetY = boxOffsetY; offsetY < boxHeight; offsetY++)
                {
                    int neighbourX = currentNode._x + dx[direction] + offsetX;
                    int neighbourY = currentNode._y + dy[direction] + offsetY;
                    if (neighbourX >= 0 && neighbourX < _grid.Width && neighbourY >= 0 && neighbourY < _grid.Height)
                    {
                        if (!_grid.GetValue(neighbourX, neighbourY)._isWalkable)
                        {
                            allWalkable = false;
                            break;
                        }
                    }
                    else
                    {
                        allWalkable = false;
                        break;
                    }
                }
                if (!allWalkable) break;
            }
            if (allWalkable)
            {
                neighbourList.Add(_grid.GetValue(currentNode._x + dx[direction], currentNode._y + dy[direction]));
            }
        }

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

    public Vector3 RandomWorldPositionWithin(float centerX, float centerY, float within)
    {
        Vector3 randomPos= new Vector3(0, 0, 0);
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
