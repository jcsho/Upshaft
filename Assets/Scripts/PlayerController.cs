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

    public GameObject gunHorizontal;

    public GameObject gunVertical;

    // Scoring Mechanic Variables
    // TODO move into separate UI manager
    public Text coinText;
    public Text scoreText;
    public Text coinTimerText;
    private float _score;
    private float _scoreTimer;
    private int coins;
    private float _coinTimer;

    

    private int _stateTimer;

    public PlatformSpawner spawner;

    private Animator _animator;
    private Rigidbody2D _rigidBody2D;
    private float _moveInput;
    private bool _isJumping;
    private bool _canDoubleJump;
    private bool _isFacingRight;
    private Weapon _weapon;
    private bool _hasGun;

    private int _state; //Powerup state, starts at 0
                        //1 for boots, 2 for gun right now

    private void Awake()
    {
        if (GameState.GameMode == "easy")
        {
            coinText.gameObject.SetActive(false);
            coinTimerText.gameObject.SetActive(false);
        }
        else if (GameState.GameMode == "normal" || GameState.GameMode == "hard")
        {
            coinText.gameObject.SetActive(true);
            coinTimerText.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _isJumping = false;
        _canDoubleJump = false;
        _isFacingRight = true;
        _weapon = GetComponent<Weapon>();
        _state = 0;
        _score = 0f;
        _scoreTimer = 0f;
        _stateTimer = 0;
        coins = 0;
        _coinTimer = coinCollectInterval;
        _hasGun = false;
    }

    private void Update()
    {   
       if (_state == 1){
           _animator.SetBool("RocketBoot", true);
           jumpForce = 475;
       }else{
           _animator.SetBool("RocketBoot", false);
           jumpForce = 400;
       }

       Movement(); 
       StateCounter();
       ScoreCounter();
       scoreText.text = "Score: " + Mathf.Round(_score);

       if (GameState.GameMode == "normal" || GameState.GameMode == "hard")
       {
           CoinCounter();

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
    }

    public bool HasGun()
    {
        return _hasGun;
    }

    public bool HasRocketBoots()
    {
        return _state == 1;
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

    private void StateCounter()
    {
        if (_stateTimer >0){
            _stateTimer --;
            if(_stateTimer<= 0){
                _state = 0;
            }
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

    public void GainPower(int pow)
    {
        Debug.Log("Collected Powerup");
        if (pow == 1){
            _state = 1;
        }
        if (pow == 2){
            gunHorizontal.SetActive(true);
            _hasGun = true;
        }
        _stateTimer = 300;
    }

    void FixedUpdate()
    {
        PhysicsMovement();
    }

    private void FireWeapon()
    {
        // check if weapon powerup enabled
        if (_hasGun == true){
            _weapon.Shoot();
        }
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
            if (_hasGun)
            {
                _animator.SetBool("isDoubleJump", true);
                gunHorizontal.SetActive(false);
                gunVertical.SetActive(true);
                FireWeapon();
                gunVertical.SetActive(false);
                gunHorizontal.SetActive(true);
            }
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