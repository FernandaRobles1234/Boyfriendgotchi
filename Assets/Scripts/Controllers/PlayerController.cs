using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterMotor _motor;

    private Animator _animator;

    private void Awake()
    {
        _motor = GetComponent<CharacterMotor>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _motor._moveDir = moveDir.normalized;

        if(moveDir != Vector2.zero)
        {
            _animator.SetBool("isWalking", true);
            _animator.SetFloat("Xinput", moveDir.x);
            _animator.SetFloat("Yinput", moveDir.y);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }

        

    }
}

