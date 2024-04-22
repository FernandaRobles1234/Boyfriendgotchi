using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNonWalkable : MonoBehaviour
{
    public GameObject _goGrid;
    private TestGrid _testGrid;
    public int _sqrSize= 0;
    private int _newX;
    private int _newY;

    private void Awake()
    {
        _testGrid = _goGrid.GetComponent<TestGrid>();
    }
    void Start()
    {
        _testGrid._grid.GetXY(transform.position, out _newX, out _newY);

        _testGrid._grid.SetValue(_newX, _newY, 1);
        print(_testGrid._grid.GetValue(_newX, _newY)); ;
    }

    void Update()
    {
        
    }
}
