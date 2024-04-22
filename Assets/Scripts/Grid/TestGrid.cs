using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    public GridClass<int> _grid;

    private void Awake()
    {
        _grid = new GridClass<int>(40, 15, transform.position, 2f, (GridClass<int> g, int x, int y) => 0);
    }
    void Start()
    {
        _grid.DebugDrawGrid();
    }
}
