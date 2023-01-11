using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpVelocity;

    private MoveState _moveState;
    private Rigidbody2D _rigidbody;
    private bool isGrounded;


    private enum MoveState
    {
        Idle, 
        Run, 
        Jump
    }
    
    void Start()
    {
        _moveState = MoveState.Idle;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
            _moveState = MoveState.Run;
        if (Input.GetKeyDown(KeyCode.Space))
            _moveState = MoveState.Jump;
    }


    private void FixedUpdate()
    {
        if (_moveState == MoveState.Run)
        {
            Run(Input.GetAxis("Horizontal"));
        }

        if (_moveState == MoveState.Jump && isGrounded)
        {
            Jump();
        }

        if (!isGrounded)
            _rigidbody.velocity -= new Vector2(0, Physics.gravity.magnitude * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        isGrounded = false;
        _rigidbody.velocity = new Vector2(0, _jumpVelocity);
    }

    private void Run(float direction) 
    {
        var newPosition = new Vector3(_moveSpeed * direction, 0, 0);
        transform.position += newPosition * Time.fixedDeltaTime;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
            isGrounded = true;
    }

    
}
