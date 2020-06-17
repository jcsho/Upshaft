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

    [Tooltip("Time to collect coin before game over")]
    public int coinCollectInterval;

    public EnemyController boss;

    // Scoring Mechanic Variables
    // TODO move into separate UI manager
    public Text coinText;
    public Text scoreText;
    public Text coinTimerText;
    private float _score;
    private float _scoreTimer;
    private int coins;
    private float _coinTimer;

    public PlatformSpawner spawner;

    private Animator _animator;
    private Rigidbody2D _rigidBody2D;
    private float _moveInput;
    private bool _isJumping;
    private bool _canDoubleJump;
    private bool _isFacingRight;
    private Weapon _weapon;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _isJumping = false;
        _canDoubleJump = false;
        _isFacingRight = true;
        _weapon = GetComponent<Weapon>();

        _score = 0f;
        _scoreTimer = 0f;
        coins = 0;
        _coinTimer = coinCollectInterval;
    }

    private void Update()
    {
       Movement(); 
       
       ScoreCounter();
       
       CoinCounter();
       
       scoreText.text = "Score: " + Mathf.Round(_score);
       coinText.text = "Coins: " + Mathf.Round(coins);

       if (_coinTimer >= 0)
       {
           coinTimerText.text = Mathf.Round(_coinTimer) + " (s)";
       }
       else
       {
           coinTimerText.text = "0 (s)";
       }
    }

    public void IncreaseScore(int amount)
    {
        _score += amount;
    }

    private void CoinCounter()
    {
        _coinTimer -= Time.deltaTime;
        if (_coinTimer < 0)
        {
            // boss kills player
            // Debug.Log("Game Over");
            boss.EndGame();
        }
    }

    private void ScoreCounter()
    {
        _scoreTimer += Time.deltaTime;
        if (_scoreTimer > 1.0f)
        {
            _score++;
            _scoreTimer = 0;
        }

    }
    // Every 3 coins the player collects will speed up the game
    public void CoinCount()
    {
        _coinTimer = coinCollectInterval;
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
        
        _animator.SetFloat("Speed", Mathf.Abs(_moveInput));

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

        _animator.SetBool("IsJumping", _isJumping);
    }

    private void PhysicsMovement()
    {
        float moveSpeed = _isJumping ? speed * 0.8f : speed;
        _rigidBody2D.velocity = new Vector2(_moveInput * moveSpeed, _rigidBody2D.velocity.y);

        // check if the character is on a platform
        // TODO - use circle cast to cover entire bottom area of character
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, tolerance, groundLayer);
        _isJumping = hit.collider == null;
        
        if (_moveInput > 0 && !_isFacingRight)
        {
            Flip();
        }
        else if (_moveInput < 0 && _isFacingRight)
        {
            Flip();
        }
    }

    private void Jump(float jumpAmount)
    {
        _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0);
        _rigidBody2D.AddForce(Vector2.up * jumpAmount);
    }

    private void Flip()
    {
        // Flip the direction the player is facing
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0f, 180f, 0f);
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