using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected GameObject _go;
    protected StateMachine _sm;


    public State(GameObject go, StateMachine sm)
    {
        _go=go;
        _sm=sm;
    }

    public virtual void Enter()
    {}

    public virtual void Update()
    {}

    public virtual void FixedUpdate()
    {}

    public virtual void Exit()
    {}
}
