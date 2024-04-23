using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class GridNonWalkable<T> : MonoBehaviour
{
    public GameObject _goGrid;
    public BoxCollider2D _boxCollider;

    private GridClass<T> _grid;
    private int _newX;
    private int _newY;

    public T _nonWalkableValue;

    private void Awake()
    {
        _grid = _goGrid.GetComponent<GridClass<T>>();
    }
    void Start()
    {
        Vector3 boundsInMeters = _boxCollider.bounds.size;

        
        _grid.GetXY(transform.position - boundsInMeters * 0.5f, out _newX, out _newY);

        for (int i = 0; i <= Mathf.CeilToInt(boundsInMeters.x/(_grid.cellSize)); i++)
        {
            for(int j = 0; j < Mathf.CeilToInt(boundsInMeters.y / (_grid.cellSize)); j++)
            {
                _grid.SetValue(_newX + i, _newY + j, _nonWalkableValue);
                //print($"At {_newX + i}, {_newY + j}, val= {_testGrid._grid.GetValue(_newX + i, _newY + j)}");
            }
        }
    }
    void Update()
    {
        
    }
}
