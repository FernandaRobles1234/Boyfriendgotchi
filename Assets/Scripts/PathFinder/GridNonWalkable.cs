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
    private GridClass<PathNode> _grid;
    private int _newX;
    private int _newY;

    public PathNode _nonWalkableValue;

    void Start()
    {
        _pathFinding = _goPathFinding.GetComponent<InitPathFinding>()._pathFinding;
        _grid = _pathFinding._grid;


        Vector3 boundsInMeters = _boxCollider.bounds.size;

        
        _grid.GetXY(transform.position - boundsInMeters * 0.5f, out _newX, out _newY);

        for (int i = 0; i <= Mathf.FloorToInt(boundsInMeters.x/(_grid.cellSize)); i++)
        {
            for(int j = 0; j <= Mathf.FloorToInt(boundsInMeters.y / (_grid.cellSize)); j++)
            {

                _nonWalkableValue= _grid.GetValue(_newX + i, _newY + j) ;
                _nonWalkableValue._isWalkable = false;

                print($"x= {_newX + i}, y= {_newY + j}");
                print($"val= {_grid.GetValue(_newX + i, _newY + j)}");
            }
        }
    }

}
