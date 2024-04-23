using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class GridNonWalkable : MonoBehaviour
{
    public GameObject _goGrid;
    public BoxCollider2D _boxCollider;

    private TestGrid _testGrid;
    private int _newX;
    private int _newY;

    

    private void Awake()
    {
        _testGrid = _goGrid.GetComponent<TestGrid>();
    }
    void Start()
    {
        Vector3 boundsInMeters = _boxCollider.bounds.size;

        
        _testGrid._grid.GetXY(transform.position - boundsInMeters * 0.5f, out _newX, out _newY);

        for (int i = 0; i <= Mathf.CeilToInt(boundsInMeters.x/(_testGrid._grid.cellSize)); i++)
        {
            for(int j = 0; j < Mathf.CeilToInt(boundsInMeters.y / (_testGrid._grid.cellSize)); j++)
            {
                _testGrid._grid.SetValue(_newX + i, _newY + j, 1);
                print($"At {_newX + i}, {_newY + j}, val= {_testGrid._grid.GetValue(_newX + i, _newY + j)}");
            }
        }
    }

    void Update()
    {
        
    }
}
