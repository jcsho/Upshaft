using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlatformSpawner : MonoBehaviour
{

    public Platform platformPrefab;

    public Vector2 spawnPosition;

    public float spawnPositionTolerance;

    public int platformSpeed;

    public float spawnInterval;

    private float _timer;
    
    // Start is called before the first frame update
    void Start()
    {
        _timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
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
