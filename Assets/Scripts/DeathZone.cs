using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {   
             
            Platform p = other.gameObject.GetComponent<Platform>();
            p.Death();
            Debug.Log("Platform removed!");
        }
    }
}
