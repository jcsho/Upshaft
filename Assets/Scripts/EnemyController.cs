using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public int moveSpeed;
    public float movementTolerance = 2.0f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody2D;
    private int _direction;
    private Vector2 _initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialPosition = transform.position;
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _direction = 1;
    }

    private void FixedUpdate()
    {
        PhysicsMovement();
    }

    private void PhysicsMovement()
    {
        if (Math.Abs(_rigidBody2D.position.x) > (_initialPosition.x + movementTolerance))
            _direction = -_direction;
        
        _rigidBody2D.velocity = new Vector2(_direction * moveSpeed, _rigidBody2D.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            StartCoroutine(FlashSprite(2, 0.2f));
        }
    }

    private IEnumerator FlashSprite(int seconds, float delay)
    {
        Color originalColor = _spriteRenderer.color;
        Color flashColor = Color.white;
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
