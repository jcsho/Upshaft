using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    [SerializeField]
    private int _moveSpeed;

    private Rigidbody2D _rigidBody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        _rigidBody2D.velocity = Vector2.down * _moveSpeed;
    }

    public void SetSpeed(int amount)
    {
        _moveSpeed = amount;
    }
}
