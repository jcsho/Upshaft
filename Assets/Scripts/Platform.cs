using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    [SerializeField]
    private int _moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate(Vector2.down * (_moveSpeed * Time.deltaTime));
    }

    public void SetSpeed(int amount)
    {
        _moveSpeed = amount;
    }
}
