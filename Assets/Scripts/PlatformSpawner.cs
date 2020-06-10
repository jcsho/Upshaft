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
    
    public int platformStartSpeed;
    [Tooltip("Amount of time per speed increase")]
    public int platformRampSpeedRatio;

    [Tooltip("Amount of time between platform spawns (in seconds)")]
    public float platformSpawnInterval;
   
    [Tooltip("Amount of platforms per coin spawn (in platforms)")]
    public int coinSpawnInterval;

    private float _timer;
    private float _spawnSpeedTimer;
    private float _platformSpawnInterval;
    private int _platformSpawnCounter;
    private int _platformSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        _timer = 0f;
        _spawnSpeedTimer = 0f;
        _platformSpawnInterval = platformSpawnInterval;
        _platformSpawnCounter = 0;
        _platformSpeed = platformStartSpeed;
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

        _spawnSpeedTimer += Time.deltaTime;
        if (_spawnSpeedTimer >= platformRampSpeedRatio)
        {
            IncreasePlatformSpeed();
            _spawnSpeedTimer = 0;
        }
    }

    private void IncreasePlatformSpeed()
    {
        if (_platformSpawnInterval >= 0.2f) _platformSpawnInterval *= 0.8f;
        _platformSpeed++;
    }

    private void SpawnPlatform()
    {
        int spawnSpeed = Mathf.RoundToInt(Mathf.Log(_platformSpeed, 2));
        Vector2 spawnLocation = new Vector2(Random.Range(spawnPosition.x - spawnPositionTolerance, spawnPosition.x + spawnPositionTolerance), spawnPosition.y);
        Platform platform = Instantiate(platformPrefab, spawnLocation, Quaternion.identity);
        platform.SetSpeed(spawnSpeed);
        
        if (_platformSpawnCounter > coinSpawnInterval)
        {
            spawnLocation.y = spawnPosition.y + 1;
            Coin coin = Instantiate(coinPrefab, spawnLocation, Quaternion.identity);
            coin.SetSpeed(spawnSpeed);
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
