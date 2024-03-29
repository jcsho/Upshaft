﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlatformSpawner : MonoBehaviour
{

    public PlayerController player;

    public Platform platformPrefab;

    public Coin coinPrefab;

    public FallingRock rockPrefab;

    public PowerUp gunPrefab;

    public PowerUp bootsPrefab;

    public Vector2 spawnPosition;

    public float spawnPositionTolerance;
    
    public int platformStartSpeed;

    public int rockStartSpeed;
    
    [Tooltip("Amount of time per speed increase")]
    public int platformRampSpeedRatio;

    [Tooltip("Amount of time between platform spawns (in seconds)")]
    public float platformSpawnInterval;
   
    [Tooltip("Amount of platforms per coin spawn (in platforms)")]
    public int coinSpawnInterval;

    public int powerSpawnInterval;

    public float rockSpawnInterval;

    private float _rockSpawnTimer;
    private float _rockSpawnInterval;
    private int _rockSpeed;
    private float _timer;
    private float _spawnSpeedTimer;
    private float _platformSpawnInterval;
    private int _platformSpawnCounter;
    private int _platformSpeed;
    private ArrayList pList = new ArrayList();
    private bool _spawnPlatforms;
    
    // Start is called before the first frame update
    void Start()
    {
        _rockSpawnTimer = 0f;
        _rockSpawnInterval = rockSpawnInterval;
        _rockSpeed = rockStartSpeed;
        _timer = 0f;
        _spawnSpeedTimer = 0f;
        _platformSpawnInterval = platformSpawnInterval;
        _platformSpawnCounter = 0;
        _platformSpeed = platformStartSpeed;
        _spawnPlatforms = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_spawnPlatforms)
        {
            _timer += Time.deltaTime;
            if (_timer >= _platformSpawnInterval)
            {
                SpawnPlatform();
                _timer = 0;
            }
        }

        if (GameState.GameMode == "hard")
        {
            _rockSpawnTimer += Time.deltaTime;
            if (_rockSpawnTimer >= _rockSpawnInterval)
            {
                SpawnRocks();
                _rockSpawnTimer = 0;
            }
        }
    }

    public void SetSpawnerActive(bool state)
    {
        _spawnPlatforms = state;
    }

    public void IncreasePlatformSpeed()
    {
        _platformSpawnInterval *= 0.8f;
        _platformSpeed++;
        foreach(Platform p in pList){
            p.SetSpeed(_platformSpeed);
        }
    }

    private void SpawnPlatform()
    {
        int spawnSpeed = _platformSpeed;
        Vector2 spawnLocation = new Vector2(Random.Range(spawnPosition.x - spawnPositionTolerance, spawnPosition.x + spawnPositionTolerance), spawnPosition.y);
        Platform platform = Instantiate(platformPrefab, spawnLocation, Quaternion.identity);
        platform.Initialize(pList);
        platform.SetSpeed(spawnSpeed);
        pList.Add(platform);


        if (GameState.GameMode == "normal" || GameState.GameMode == "hard")
        {
            SpawnCoin(spawnLocation, spawnSpeed);
            SpawnPowerUp(spawnLocation,spawnSpeed);
        }

        
        
    }

    public void IncreaseRockSpeed()
    {
        _rockSpeed++;
        _rockSpawnInterval *= 0.8f;
    }

    private void SpawnRocks()
    {
        int spawnSpeed = _rockSpeed;
        Vector2 spawnLocation = new Vector2(Random.Range(spawnPosition.x - spawnPositionTolerance, spawnPosition.x + spawnPositionTolerance), spawnPosition.y);
        FallingRock rock = Instantiate(rockPrefab, spawnLocation, Quaternion.identity);
        rock.SetSpeed(spawnSpeed);
    }

    private void SpawnCoin(Vector2 location, int speed)
    {
        if (_platformSpawnCounter > coinSpawnInterval)
        {
            location.y = spawnPosition.y + 1;
            Coin coin = Instantiate(coinPrefab, location, Quaternion.identity);
            coin.SetSpeed(speed);
            _platformSpawnCounter = 0;
        }
        
        _platformSpawnCounter++;
    }

    private void SpawnPowerUp(Vector2 location, int speed)
    {
        if(_platformSpawnCounter > powerSpawnInterval)
        {
           Debug.Log("Spawning powerup"); 
           int i = Random.Range(0,3);
           location.y = spawnPosition.y + 1.5f;
           if (i<2 && !player.HasRocketBoots()){
                PowerUp p = Instantiate(bootsPrefab,location,Quaternion.identity);
                p.setType(1);
                p.SetSpeed(speed);
           }else if (i==2 && !player.HasGun()){
                PowerUp p = Instantiate(gunPrefab,location,Quaternion.identity);
                p.setType(2);
                p.SetSpeed(speed);
           }
           
           _platformSpawnCounter = 0; 
        }
        _platformSpawnCounter ++;
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

internal struct NewStruct
{
    public object Item1;
    public object Item2;

    public NewStruct(object item1, object item2)
    {
        Item1 = item1;
        Item2 = item2;
    }

    public override bool Equals(object obj)
    {
        return obj is NewStruct other &&
               EqualityComparer<object>.Default.Equals(Item1, other.Item1) &&
               EqualityComparer<object>.Default.Equals(Item2, other.Item2);
    }

    public override int GetHashCode()
    {
        int hashCode = -1030903623;
        hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Item1);
        hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Item2);
        return hashCode;
    }

    public void Deconstruct(out object item1, out object item2)
    {
        item1 = Item1;
        item2 = Item2;
    }

    public static implicit operator (object, object)(NewStruct value)
    {
        return (value.Item1, value.Item2);
    }

    public static implicit operator NewStruct((object, object) value)
    {
        return new NewStruct(value.Item1, value.Item2);
    }
}