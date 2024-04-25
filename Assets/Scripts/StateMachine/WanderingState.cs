using BehaviorDesigner.Runtime.Tasks.Unity.UnityTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingState : State
{
    CharacterMotor _charMotor;
    Animator _animator;
    AIController _controller;

    public const float _time= 2.0f;
    private float _timeSinceLast;

   public WanderingState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    public override void Enter()
    {
        _charMotor = _go.GetComponent<CharacterMotor>();
        _animator = _go.GetComponent<Animator>();
        _controller = _go.GetComponent<AIController>();

        _timeSinceLast = 0.0f;
    }


    public override void FixedUpdate()
    {
        _timeSinceLast += Time.fixedDeltaTime;

        if (_time < _timeSinceLast)
        {
            _charMotor._moveDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            _charMotor._moveDir.Normalize();
            _timeSinceLast = 0.0f;

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

    }
    public override void Exit()
    {
        _animator.SetBool("isWalking", false);
    }
}
