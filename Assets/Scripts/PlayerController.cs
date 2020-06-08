using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Tooltip("Movement speed")]
    [Range(1, 20)]
    public int speed;
    
    [Tooltip("Power of Jump")]
    [Range(400, 1000)]
    public int jumpForce;

    private Rigidbody2D _rigidBody2D;
    private float _moveInput;
    private bool _isJumping;
    private bool _canDoubleJump;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _isJumping = false;
        _canDoubleJump = false;
    }

    private void Update()
    {
       Movement(); 
    }

    void FixedUpdate()
    {
        PhysicsMovement();
    }

    private void Movement()
    {
        _moveInput = Input.GetAxis("Horizontal");

        if (_rigidBody2D.velocity.y == 0)
            _isJumping = false;
        else
            _isJumping = true;

        if (!_isJumping) _canDoubleJump = true;

        if (!_isJumping && Input.GetButtonDown("Jump"))
        {
            Jump(jumpForce);
        }
        else if (_canDoubleJump && Input.GetButtonDown("Jump"))
        {
            Jump(jumpForce * 0.85f);
            _canDoubleJump = false;
        }
    }

    private void PhysicsMovement()
    {
        float moveSpeed = _isJumping ? speed * 0.5f : speed;
        _rigidBody2D.velocity = new Vector2(_moveInput * moveSpeed, _rigidBody2D.velocity.y);
    }

    private void Jump(float jumpAmount)
    {
        _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0);
        _rigidBody2D.AddForce(Vector2.up * jumpAmount);
    }
}
