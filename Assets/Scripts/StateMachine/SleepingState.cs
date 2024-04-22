using BehaviorDesigner.Runtime.Tasks.Unity.UnityTime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SleepingState : State
{
    CharacterMotor _charMotor;
    Animator _animator;
    AIController _controller;

    private BedObject[] _BedObjects;

    //[SerializeField] Transform target;
    //NavMeshAgent agent;
    

    public SleepingState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    public override void Enter()
    {
        _charMotor = _go.GetComponent<CharacterMotor>();
        _animator = _go.GetComponent<Animator>();
        _controller = _go.GetComponent<AIController>();

        _BedObjects = GameObject.FindObjectsOfType<BedObject>();

        //agent = _go.GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;
        //agent.updateUpAxis = false;
    }


    public override void FixedUpdate()
    {

        _charMotor._moveDir = _BedObjects[0].transform.position - _go.transform.position;
        _charMotor._moveDir.Normalize();


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

    }
}
