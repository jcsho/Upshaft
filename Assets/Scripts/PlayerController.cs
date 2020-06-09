using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{

    [Tooltip("Movement speed")]
    [Range(1, 20)]
    public int speed;
    
    [Tooltip("Power of Jump")]
    [Range(400, 1000)]
    public int jumpForce;

    [Space(20)] [Header("Ground Detection")]
    [Tooltip("Distance to check for ground below the character")]
    public float tolerance;
    
    [Tooltip("Layer to check for ground")]
    public LayerMask groundLayer;

    private Rigidbody2D _rigidBody2D;
    private float _moveInput;
    private bool _isJumping;
    private bool _canDoubleJump;
    private Weapon _weapon;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _isJumping = false;
        _canDoubleJump = false;
        _weapon = GetComponent<Weapon>();
    }

    private void Update()
    {
       Movement(); 
       FireWeapon();
    }

    void FixedUpdate()
    {
        PhysicsMovement();
    }

    private void FireWeapon()
    {
        if (!_isJumping && _canDoubleJump && Input.GetButtonDown("Jump") /* && if weapon powerup equipped */)
        {
            _weapon.Shoot();
        }
    }

    private void Movement()
    {
        _moveInput = Input.GetAxis("Horizontal");
        
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

        // check if the character is on a platform
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, tolerance, groundLayer);
        _isJumping = hit.collider != null;
    }

    private void Jump(float jumpAmount)
    {
        _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0);
        _rigidBody2D.AddForce(Vector2.up * jumpAmount);
    }

    private void OnDrawGizmosSelected()
    {
        // show ground detection line
        Gizmos.color = Color.green;
        Vector3 position = transform.position;
        Vector3 target = new Vector3(position.x, position.y - tolerance, 0);
        Gizmos.DrawLine(position, target);
    }
}
