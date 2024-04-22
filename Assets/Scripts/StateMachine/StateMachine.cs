using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private State _curState;

    public State _CurState
    {
        get => _curState;

        set
        {
            if (_curState != null)
            {
                _curState.Exit();
            }

            _curState = value;

            _curState.Enter();
        }
    }
}
