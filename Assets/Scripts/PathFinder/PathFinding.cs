using System.Collections;
using System.Collections.Generic;
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
        _grid = new GridClass<PathNode>(width, height, origin, 2.0f, (GridClass<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
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
        List<PathNode> neighbourList= new List<PathNode>();

        if (currentNode._x - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(currentNode._x -1, currentNode._y));
            if (currentNode._y - 1 >= 0) neighbourList.Add(GetNode(currentNode._x - 1, currentNode._y -1));
            if (currentNode._y + 1 < _grid.Height) neighbourList.Add(GetNode(currentNode._x - 1, currentNode._y + 1));
        }
        if (currentNode._x + 1 < _grid.Width)
        {
            neighbourList.Add(GetNode(currentNode._x + 1, currentNode._y));
            if (currentNode._y - 1 >= 0) neighbourList.Add(GetNode(currentNode._x, currentNode._y - 1));
            if (currentNode._y + 1 < _grid.Height) neighbourList.Add(GetNode(currentNode._x, currentNode._y + 1)) ;
        }

        if (currentNode._y - 1 >= 0) neighbourList.Add(GetNode(currentNode._x, currentNode._y - 1));
        if (currentNode._y + 1 < _grid.Height) neighbourList.Add(GetNode(currentNode._x, currentNode._y + 1));

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

        return null;
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
}
