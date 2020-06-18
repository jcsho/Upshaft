using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    private int _moveSpeed;
    private int _type;
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
            if (player != null){
                player.GainPower(_type);
            } 
            Destroy(this.gameObject);
        }
    }

    public void setType(int t){
        _type = t;
    }
    public void SetSpeed(int amount)
    {
        _moveSpeed = amount;
    }
}
