using BehaviorDesigner.Runtime.Tasks.Unity.UnityTime;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class SleepyState : State
{
    

    CharacterMotor _charMotor;
    Animator _animator;
    AIController _controller;

    private BedObject[] _BedObjects;
    private InitPathFinding[] _initPathFindings;

    private List<PathNode> _pathToBed;

    Vector3 _moveDir;

    int _i = 0;

    BoxCollider2D _characterZone;
    BoxCollider2D _bedZone;

    public SleepyState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    public override void Enter()
    {
        _charMotor = _go.GetComponent<CharacterMotor>();
        _animator = _go.GetComponent<Animator>();
        _controller = _go.GetComponent<AIController>();

        _BedObjects = GameObject.FindObjectsOfType<BedObject>();
        _initPathFindings = GameObject.FindObjectsOfType<InitPathFinding>();

        Vector3 characterPosition = _go.transform.position;
        Vector3 bedPosition = _BedObjects[0].transform.position;

        _pathToBed = _initPathFindings[0]._pathFinding.FindPath(characterPosition.x, characterPosition.y - 1, bedPosition.x, bedPosition.y);

        _characterZone= _go.GetComponent<BoxCollider2D>();
        _bedZone= _BedObjects[0].GetComponent<BoxCollider2D>();
    }


    public override void FixedUpdate()
    {
        // Handle conditions for the state management.
        if (_bedZone.IsTouching(_characterZone))
        {
            _sm._CurState = new SleepingState(_go, _sm);
        }

        // Move the most inmediate cell of the grid's path.
        if (_i < _pathToBed.Count)
        {
            PathNode firstNode = _pathToBed[_i];
            Vector3 characterPosition = _go.transform.position;

            _moveDir = _initPathFindings[0]._pathFinding._grid.GetWorldPosition(firstNode._x, firstNode._y) - characterPosition - new Vector3(0, -1, 0);
            _charMotor._moveDir = _moveDir;
            _charMotor._moveDir.Normalize();
        }

        // Handling end of path.
        if (_moveDir.sqrMagnitude < 0.5)
        {
            if (_i >= _pathToBed.Count)
            {
                _charMotor._moveDir = Vector3.zero;
                _animator.SetBool("isWalking", false);
            }
            else
            {
                _i++;
            }
        }

        // Handle animations.
        if (_charMotor.IsMoving())
        {
            _animator.SetBool("isWalking", true);
            _animator.SetFloat("Xinput", _charMotor._moveDir.x);
            _animator.SetFloat("Yinput", _charMotor._moveDir.y);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }
    public override void Exit()
    {
        _charMotor._moveDir = Vector3.zero;
        _animator.SetBool("isWalking", false);
    }
}