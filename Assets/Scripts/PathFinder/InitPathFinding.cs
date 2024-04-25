using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPathFinding : MonoBehaviour
{
    public PathFinding _pathFinding;

    public int w;
    public int h;

    void Awake()
    {
        _pathFinding = new PathFinding(w, h, transform.position);
        _pathFinding._grid.DebugDrawGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
