using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AIController : NetworkBehaviour
{
    private StateMachine _sm;
    private CharacterMotor _motor;

    public float _sleepyMax = 10;
    public float _sleepy= 0;
    public float _sleepyMin = 2;
    public float _wanderingChangeTime = 5;
    public Vector2 _wanderingWithin = new Vector2(5, 20);

    public GameObject _bubbleObject;

    // only call this on server
    [ServerCallback]
    private void Awake() 
    { 
        _sm = new StateMachine();
        _sm._CurState = new WanderingState(gameObject, _sm);
    }

    void Start()
    {
    }

    // only call this on server
    [ServerCallback]
    void Update()
    {
        _sm._CurState.Update();

    }

    // only call this on server
    [ServerCallback]
    void FixedUpdate()
    {
        _sm._CurState.FixedUpdate();
    }
}
