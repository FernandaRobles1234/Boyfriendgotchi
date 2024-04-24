using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPathFinding : MonoBehaviour
{
    public PathFinding _pathFinding;
    void Awake()
    {
        _pathFinding = new PathFinding(40, 15, transform.position);
        _pathFinding._grid.DebugDrawGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
