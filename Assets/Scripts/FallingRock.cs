using System;
using UnityEngine;

public class FallingRock : MonoBehaviour
{

    public float impactForce;
    public float speed;

    private Rigidbody2D _rigidBody2D;

    private void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidBody2D.velocity = Vector2.down * speed;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            PlayerController player = other.GetComponent<PlayerController>();
            //player.HitByRock(direction, impactForce);
        }
    }
}
