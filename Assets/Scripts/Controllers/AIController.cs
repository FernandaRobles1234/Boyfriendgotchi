using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private StateMachine _sm;
    private CharacterMotor _motor;

    public float _sleepy= 0;
    public const float _sleepyMax= 2;

    private void Awake() 
    { 
        _sm = new StateMachine();
        _sm._CurState = new WanderingState(gameObject, _sm);
    }

    void Start()
    {
    }

    void Update()
    {
        _sm._CurState.Update();
    }

    void FixedUpdate()
    {
        _sm._CurState.FixedUpdate();
    }
}
