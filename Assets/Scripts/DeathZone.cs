using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathZone : MonoBehaviour
{

    public PlayerController player; 
    public PlatformSpawner spawner;
    public Text[] gameOverText;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {   
             
            Platform p = other.gameObject.GetComponent<Platform>();
            p.Death();
            Debug.Log("Platform removed!");
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

    private void Update()
    {
        if (player.gameObject.activeSelf == false && Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}

