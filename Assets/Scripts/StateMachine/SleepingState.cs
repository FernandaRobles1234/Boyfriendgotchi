using BehaviorDesigner.Runtime.Tasks.Unity.UnityTime;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class SleepingState : State
{
    CharacterMotor _charMotor;
    Animator _animator;
    AIController _controller;

    private BedObject[] _BedObjects;
    private InitPathFinding[] _initPathFindings;

    private List<PathNode> _pathToBed;

    Vector3 _moveDir;

    int _i = 0;

    public SleepingState(GameObject go, StateMachine sm) : base(go, sm)
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

        _pathToBed = _initPathFindings[0]._pathFinding.FindPath(characterPosition.x, characterPosition.y, bedPosition.x, bedPosition.y);
    }


    public override void FixedUpdate()
    { 
        if (_i < _pathToBed.Count)
        {
            PathNode firstNode = _pathToBed[_i];
            Vector3 characterPosition = _go.transform.position;

            _moveDir = _initPathFindings[0]._pathFinding._grid.GetWorldPosition(firstNode._x, firstNode._y) - characterPosition;
            _charMotor._moveDir = _moveDir;
            _charMotor._moveDir.Normalize();
        }
        

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
    }
    public override void Exit()
    {

    }
}
