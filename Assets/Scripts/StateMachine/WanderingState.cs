using BehaviorDesigner.Runtime.Tasks.Unity.UnityTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingState : State
{
    CharacterMotor _charMotor;
    Animator _animator;
    AIController _controller;

    private InitPathFinding _initPathFinding;
    private List<PathNode> _pathToRandomPos;

    private Vector3 _moveDir;

    public const float _time= 3.0f;
    private float _timeSinceLast;

    int _i = 0;

    public WanderingState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    public override void Enter()
    {
        _charMotor = _go.GetComponent<CharacterMotor>();
        _animator = _go.GetComponent<Animator>();
        _controller = _go.GetComponent<AIController>();

        _initPathFinding = GameObject.FindObjectsOfType<InitPathFinding>()[0];

        _timeSinceLast = 0.0f;

        Vector3 characterPosition = _go.transform.position;
        int within = 15;
        Vector3 randomPos = _initPathFinding._pathFinding.RandomWorldPositionWithin(characterPosition.x, characterPosition.y, within);
        Debug.Log("Character position " + characterPosition);
        Debug.Log("random position " + randomPos);

        _pathToRandomPos = _initPathFinding._pathFinding.FindPath(characterPosition.x, characterPosition.y, randomPos.x, randomPos.y);
    }


    public override void FixedUpdate()
    {
        Vector3 characterPosition = _go.transform.position;
        _timeSinceLast += Time.fixedDeltaTime;

        if (_time < _timeSinceLast)
        {
            characterPosition = _go.transform.position;
            int within = 5;
            Vector3 randomPos = _initPathFinding._pathFinding.RandomWorldPositionWithin(characterPosition.x, characterPosition.y, within);

            _pathToRandomPos = _initPathFinding._pathFinding.FindPath(characterPosition.x, characterPosition.y, randomPos.x, randomPos.y);
            if (_pathToRandomPos != null)
            {
                Debug.Log("Failure finding path");
            }

            _timeSinceLast = 0.0f;
            _i = 0;
        }


        if (_i < _pathToRandomPos.Count)
        {
            PathNode firstNode = _pathToRandomPos[_i];
            characterPosition = _go.transform.position;

            _moveDir = _initPathFinding._pathFinding._grid.GetWorldPosition(firstNode._x, firstNode._y) - characterPosition;
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

        _controller._sleepy += Time.deltaTime;

        if(_controller._sleepy > _controller._sleepyMax)
        {
            _sm._CurState = new SleepingState(_go, _sm);
        }

        if (_moveDir.sqrMagnitude < 0.5)
        {
            if (_i >= _pathToRandomPos.Count)
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
