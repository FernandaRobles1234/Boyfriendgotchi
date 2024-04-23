using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPathFinding : MonoBehaviour
{
    PathFinding _pathFinding;
    void Start()
    {
        _pathFinding = new PathFinding(40, 15, transform.position);
        _pathFinding._grid.DebugDrawGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
