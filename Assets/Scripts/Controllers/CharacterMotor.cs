using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterMotor : NetworkBehaviour
{
    public float _moveSpeed = 500;
    public Vector2 _moveDir;

    public Rigidbody2D _rb;
    private float movementThreshold = 0.1f; // Threshold to consider the Rigidbody as moving

    private void Awake()
    {

        // only simulate physics  on server
        _rb.simulated = true;
    }

    // only call this on server
    [ServerCallback]
    public void FixedUpdate()
    {
        _rb.AddForce(_moveDir * _moveSpeed * Time.fixedDeltaTime);
    }

    // only call this on server
    public bool IsMoving()
    {
        return _rb.velocity.magnitude > movementThreshold;
    }
}

