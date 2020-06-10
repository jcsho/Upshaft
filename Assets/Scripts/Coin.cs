using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private int _moveSpeed;
    private Rigidbody2D _rigidBody2D;

    private void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidBody2D.velocity = Vector2.down * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) player.IncreaseScore(10);
            Destroy(this.gameObject);
        }
    }

    public void SetSpeed(int amount)
    {
        _moveSpeed = amount;
    }
}
