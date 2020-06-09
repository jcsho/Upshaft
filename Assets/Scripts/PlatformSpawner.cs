using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlatformSpawner : MonoBehaviour
{

    public Platform platformPrefab;

    public Coin coinPrefab;

    public Vector2 spawnPosition;

    public float spawnPositionTolerance;

    public int platformSpeed;

    [Tooltip("Amount of time between platform spawns (in seconds)")]
    public float platformSpawnInterval;
   
    [Tooltip("Amount of platforms per coin spawn (in platforms)")]
    public int coinSpawnInterval;

    private float _timer;
    private int _platformSpawnCounter;
    
    // Start is called before the first frame update
    void Start()
    {
        _timer = 0f;
        _platformSpawnCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= platformSpawnInterval)
        {
            SpawnPlatform();
            _timer = 0;
        }
    }

    private void SpawnPlatform()
    {
        Vector2 spawnLocation = new Vector2(Random.Range(spawnPosition.x - spawnPositionTolerance, spawnPosition.x + spawnPositionTolerance), spawnPosition.y);
        Platform platform = Instantiate(platformPrefab, spawnLocation, Quaternion.identity);
        platform.SetSpeed(platformSpeed);
        
        if (_platformSpawnCounter > coinSpawnInterval)
        {
            spawnLocation.y = spawnPosition.y + 1;
            Coin coin = Instantiate(coinPrefab, spawnLocation, Quaternion.identity);
            coin.SetSpeed(platformSpeed);
            _platformSpawnCounter = 0;
        }
        
        _platformSpawnCounter++;
    }

    private void OnDrawGizmosSelected()
    {
        // draw spawn position tolerance
       Gizmos.color = Color.green;
       Vector3 toleranceStart = new Vector3(spawnPosition.x - spawnPositionTolerance, spawnPosition.y, 0);
       Vector3 toleranceEnd = new Vector3(spawnPosition.x + spawnPositionTolerance, spawnPosition.y, 0);
       Gizmos.DrawLine(toleranceStart, toleranceEnd);
    }
}
