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



    public const float _time= 5.0f;
    private float _timeSinceLast= 6.0f;
    private int _iPath = 0;

    public WanderingState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    public override void Enter()
    {
        _charMotor = _go.GetComponent<CharacterMotor>();
        _animator = _go.GetComponent<Animator>();
        _controller = _go.GetComponent<AIController>();

        _initPathFinding = GameObject.FindObjectsOfType<InitPathFinding>()[0];
    }


    public override void FixedUpdate()
    {
        _timeSinceLast += Time.fixedDeltaTime;
        Vector3 characterPosition = _go.transform.position;
        Vector3 _toPosDir= Vector3.zero;
        
        // Pick and create a new path.
        if (_time < _timeSinceLast)
        {
            Vector2 within = new Vector2(5, 20);
            Vector3 randomPos = _initPathFinding._pathFinding.RandomWorldPositionWithin(characterPosition.x, characterPosition.y, within);

            characterPosition = _go.transform.position;

            _pathToRandomPos = _initPathFinding._pathFinding.FindPath(characterPosition.x, characterPosition.y - 1, randomPos.x, randomPos.y);

            _timeSinceLast = 0.0f;
            _iPath = 0;
        }


        // Move the most inmediate cell of the grid's path.
        if (_iPath < _pathToRandomPos.Count)
        {
            PathNode firstNode = _pathToRandomPos[_iPath];
            characterPosition = _go.transform.position;

            _toPosDir = _initPathFinding._pathFinding._grid.GetWorldPosition(firstNode._x, firstNode._y) - characterPosition - new Vector3(0, -1, 0);
            _charMotor._moveDir = _toPosDir;
            _charMotor._moveDir.Normalize();
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


        // Handle conditions for the state management.
        _controller._sleepy += Time.deltaTime;

        if(_controller._sleepy > _controller._sleepyMax)
        {
            _sm._CurState = new SleepingState(_go, _sm);
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

    }
    public override void Exit()
    {
        _charMotor._moveDir = Vector3.zero;
        _animator.SetBool("isWalking", false);
    }
}
