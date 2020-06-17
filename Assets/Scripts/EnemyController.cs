using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    public int moveSpeed;
    public float movementTolerance = 2.0f;
    public PlayerController player;
    public PlatformSpawner spawner;
    public Text[] gameOverText;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody2D;
    private int _direction;
    private Vector2 _initialPosition;
    private int _speed;

    void Awake()
    {
        if (GameState.GameMode == "easy")
        {
            gameObject.SetActive(false);
        }
        else if (GameState.GameMode == "normal" || GameState.GameMode == "hard")
        {
            gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialPosition = transform.position;
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _direction = 1;
        _speed = 0;
    }
    

    private void FixedUpdate()
    {
       PhysicsMovement(); 
    }

    public void EndGame()
    {
        _speed = moveSpeed;
    }
    
    private void PhysicsMovement()
    {
        if (_speed > 0)
            _rigidBody2D.velocity = new Vector2(0f, _direction * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            player.IncreaseScore(2);
            StartCoroutine(FlashSprite(2, 0.2f));
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            spawner.SetSpawnerActive(false);
            foreach (Text text in gameOverText)
            {
                text.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator FlashSprite(int seconds, float delay)
    {
        Color originalColor = _spriteRenderer.color;
        Color flashColor = Color.red;
        for (int cycle = 0; cycle < seconds; cycle++)
        {
            _spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(delay);
            _spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        
        Vector3 toleranceStart = new Vector3(_initialPosition.x - movementTolerance, _initialPosition.y, 0);
        Vector3 toleranceEnd = new Vector3(_initialPosition.x + movementTolerance, _initialPosition.y, 0);
        Gizmos.DrawLine(toleranceStart, toleranceEnd);
    }
}
