using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPathFinding : MonoBehaviour
{
    public PathFinding _pathFinding;
    private bool _debug;

    public int w;
    public int h;

    void Awake()
    {
        _pathFinding = new PathFinding(w, h, transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.R)) // Press D to toggle debug drawing
        {
            _debug = !_debug;
        }

        if (_debug)
        {
            _pathFinding._grid.DebugDrawGrid();
        }
        
    }
}
