using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _rigidbody2d;
    private float _moveInput;
    private float _speed = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _moveInput = Input.GetAxis("Horizontal");
        _rigidbody2d.velocity = new Vector2(_moveInput * _speed, _rigidbody2d.velocity.y);
    }
}
