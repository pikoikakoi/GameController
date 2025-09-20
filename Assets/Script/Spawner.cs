using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject starPrefab;
    public GameObject bubblePrefab;
    public GameObject[] obstaclePrefabs;

    [Header("Probabilities")]
    [Range(0f, 1f)] public float starChance = 0.5f;
    [Range(0f, 1f)] public float bubbleChance = 0.3f;
    [Range(0f, 1f)] public float obstacleChance = 0.2f;

    [Header("Spawn Settings")]
    public float spawnInterval = 1f; // jeda antar spawn

    private float timer;
    private bool isPlaying;

    private void OnEnable()
    {
        GameManager.OnStateChanged += StateHandler;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= StateHandler;
    }

    void Update()
    {
        if (!isPlaying) return;
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    private void StateHandler(GameState newState)
    {
        if (newState == GameState.Ingame)
        {
            isPlaying = true;
        }
    }

    void SpawnObject()
    {
        // titik kanan layar
        float screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        // tinggi layar (y bawah - y atas)
        float screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // random posisi Y
        float randomY = Random.Range(screenBottom, screenTop);

        Vector3 spawnPos = new Vector3(screenRight, randomY, 0);

        // random probabilitas
        float roll = Random.value; // antara 0 - 1
        GameObject prefabToSpawn = null;

        if (roll < starChance)
        {
            prefabToSpawn = starPrefab;
        }
        else if (roll < starChance + bubbleChance)
        {
            prefabToSpawn = bubblePrefab;
        }
        else
        {
            if (obstaclePrefabs.Length > 0)
            {
                prefabToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            }
        }

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        }
    }
}
