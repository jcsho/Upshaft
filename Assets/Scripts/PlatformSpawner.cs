using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlatformSpawner : MonoBehaviour
{

    public Platform platformPrefab;

    public Vector2 spawnPosition;

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
        Platform platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        platform.SetSpeed(platformSpeed);
    }
}
