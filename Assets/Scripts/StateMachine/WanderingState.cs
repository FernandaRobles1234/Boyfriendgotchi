using BehaviorDesigner.Runtime.Tasks.Unity.UnityTime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WanderingState : State
{
    CharacterMotor _charMotor;
    AIController _controller;
    Animator _animator;
    InitPathFinding _initPathFinding;

    private float _timeSinceLast= 0.0f;
    private int _iPath = 0;
    private List<PathNode> _pathToRandomPos;

    private int _offset= -1;

    public WanderingState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }
    public override void Enter()
    {
        _charMotor = _go.GetComponent<CharacterMotor>();
        _animator = _go.GetComponent<Animator>();
        _controller = _go.GetComponent<AIController>();
        _initPathFinding = GameObject.FindObjectsOfType<InitPathFinding>()[0];

        _timeSinceLast = _controller._wanderingChangeTime + 1;
    }

    public override void FixedUpdate()
    {
        _timeSinceLast += Time.fixedDeltaTime;
        _controller._sleepy += Time.deltaTime;

        Vector3 characterPosition = _go.transform.position;
        Vector3 _toPosDir= Vector3.zero;

        // Handle conditions for the state management.
        if (_controller._sleepy > _controller._sleepyMax)
        {
            _sm._CurState = new SleepyState(_go, _sm);
        }

        // Pick and create a new path.
        if (_controller._wanderingChangeTime < _timeSinceLast)
        {
            Vector3 randomPos = _initPathFinding._pathFinding.RandomWorldPositionWithin(characterPosition.x, characterPosition.y + _offset, _controller._wanderingWithin);

            _pathToRandomPos = _initPathFinding._pathFinding.FindPath(characterPosition.x, characterPosition.y + _offset, randomPos.x, randomPos.y);

            _timeSinceLast = 0.0f;
            _iPath = 0;
        }

        // Move the most inmediate cell of the grid's path.
        if (_iPath < _pathToRandomPos.Count)
        {
            PathNode firstNode = _pathToRandomPos[_iPath];

            _toPosDir = _initPathFinding._pathFinding._grid.GetWorldPosition(firstNode._x, firstNode._y) - characterPosition - new Vector3(0, _offset, 0);
            _charMotor._moveDir = _toPosDir;
            _charMotor._moveDir.Normalize();
        }

        // Handling end of path.
        if (_toPosDir.sqrMagnitude < 0.5)
        {
            if (_iPath >= _pathToRandomPos.Count)
            {
                _charMotor._moveDir = Vector3.zero;
                _animator.SetBool("isWalking", false);
            }
            else
            {
                _iPath++;
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
