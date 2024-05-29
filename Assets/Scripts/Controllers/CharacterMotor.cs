using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    public float _moveSpeed = 500;
    public Vector2 _moveDir;

    private Rigidbody2D _rb;
    private float movementThreshold = 0.1f; // Threshold to consider the Rigidbody as moving

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

    }

    public void FixedUpdate()
    {
        _rb.AddForce(_moveDir * _moveSpeed * Time.fixedDeltaTime);
    }

    public bool IsMoving()
    {
        return _rb.velocity.magnitude > movementThreshold;
    }
}

