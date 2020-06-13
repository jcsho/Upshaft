using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    // Scoring Mechanic Variables
    // TODO move into separate UI manager
    public Text scoreText;
    private float _score;
    private float _scoreTimer;
    private int coins;

    public PlatformSpawner spawner;

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

        _score = 0f;
        _scoreTimer = 0f;
        coins = 0;
    }

    private void Update()
    {
       Movement(); 
       
       ScoreCounter();
    }

    public void IncreaseScore(int amount)
    {
        _score += amount;
    }

    private void ScoreCounter()
    {
        _scoreTimer += Time.deltaTime;
        if (_scoreTimer > 1.0f)
        {
            _score++;
            _scoreTimer = 0;
        }

        scoreText.text = "Score: " + Mathf.Round(_score);
    }
    // Every 3 coins the player collects will speed up the game
    public void CoinCount(){
        coins ++;
        IncreaseScore(10);
        if (coins%3 == 0){
            spawner.IncreasePlatformSpeed();
        }
        
    }

    void FixedUpdate()
    {
        PhysicsMovement();
    }

    private void FireWeapon()
    {
        // check if weapon powerup enabled
        _weapon.Shoot();
    }

    private void Movement()
    {
        _moveInput = Input.GetAxis("Horizontal");
        
        if (!_isJumping && Input.GetButtonDown("Jump"))
        {
            Jump(jumpForce);
            _canDoubleJump = true;
        }
        else if (_canDoubleJump && Input.GetButtonDown("Jump"))
        {
            Jump(jumpForce * 0.85f);
            _canDoubleJump = false;
            FireWeapon();
        }
    }

    private void PhysicsMovement()
    {
        float moveSpeed = _isJumping ? speed * 0.8f : speed;
        _rigidBody2D.velocity = new Vector2(_moveInput * moveSpeed, _rigidBody2D.velocity.y);

        // check if the character is on a platform
        // TODO - use circle cast to cover entire bottom area of character
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, tolerance, groundLayer);
        _isJumping = hit.collider == null;
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
