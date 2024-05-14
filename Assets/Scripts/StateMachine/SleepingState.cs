using BehaviorDesigner.Runtime.Tasks.Unity.UnityTime;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SleepingState : State
{
    CharacterMotor _charMotor;
    Animator _animator;
    AIController _controller;

    Animator _bubbleAnimator;


    public SleepingState(GameObject go, StateMachine sm) : base(go, sm)
    {
        _charMotor = _go.GetComponent<CharacterMotor>();
        _animator = _go.GetComponent<Animator>();
        _controller = _go.GetComponent<AIController>();

        _controller._bubbleObject.SetActive(true);
        _bubbleAnimator = _controller._bubbleObject.GetComponent<Animator>();
    }

    public override void Enter()
    {
        _bubbleAnimator.SetBool("isSleeping", true);
    }
    public override void FixedUpdate()
    {
        _charMotor._moveDir = Vector3.zero;
        _animator.SetBool("isWalking", false);

        _controller._sleepy -= Time.deltaTime;

        // Handle conditions for the state management.
        if (_controller._sleepy < _controller._sleepyMin)
        {
            _controller._sleepy = 0;
            _sm._CurState = new WanderingState(_go, _sm);
        }
    }
    public override void Exit()
    {
        _bubbleAnimator.SetBool("isSleeping", false);
        _controller._bubbleObject.SetActive(false);
    }
}
