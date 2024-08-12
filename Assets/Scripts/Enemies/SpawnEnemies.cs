using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // List of enemy prefabs to spawn
    public float spawnInterval = 2f; // Time interval between spawns
    public float spawnDelay = 1f; // Initial delay before starting to spawn

    public List<Transform> spawnPoints; // List of spawn points

    public int maxEnemies = 10; // Maximum number of enemies to spawn
    private int currentEnemyCount = 0;
    public int enemiesAlive = 0;
    public bool isSpawningActive; // Controls whether spawning is active
    public bool lastEnemySpawned = false; // Flag to indicate the last enemy has been spawned

    private Coroutine spawnCoroutine; // Track the coroutine
    EnemyManager enemyManager;

    void Start()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        isSpawningActive = true;
        lastEnemySpawned = false;
        enemiesAlive = maxEnemies;
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null) // Ensure coroutine is not already running
        {
            isSpawningActive = true;
            currentEnemyCount = 0;
            lastEnemySpawned = false;
            spawnCoroutine = StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(spawnDelay);

        while (currentEnemyCount < maxEnemies && isSpawningActive)
        {
            // Randomly choose one of the spawn points
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            
            // Randomly choose one of the enemy prefabs
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            // Calculate random offset based on the spawn point
            Vector3 offset = CalculateRandomOffset(spawnPoint);
            
            // Instantiate the enemy at the chosen spawn position with the random offset
            Instantiate(enemyPrefab, spawnPoint.position + offset, Quaternion.identity);
            enemyManager.AddEnemy(enemyPrefab);

            currentEnemyCount++;

            if (currentEnemyCount >= maxEnemies)
            {
                lastEnemySpawned = true;
                isSpawningActive = false;
            }

            // Wait for the next spawn
            yield return new WaitForSeconds(spawnInterval);
        }

        spawnCoroutine = null; // Reset coroutine reference when done
    }

    Vector3 CalculateRandomOffset(Transform spawnPoint)
    {
        // Check which spawn point it is and calculate the offset accordingly
        if (spawnPoint.name == "SpawnPointLeft" || spawnPoint.name == "SpawnPointRight")
        {
            // Random offset for Left and Right spawns on the y-axis
            return new Vector3(0, Random.Range(-2.5f, 2.5f), 0);
        }
        else if (spawnPoint.name == "SpawnPointTop" || spawnPoint.name == "SpawnPointBot")
        {
            // Random offset for Top and Bottom spawns on the x-axis
            return new Vector3(Random.Range(-2.5f, 2.5f), 0, 0);
        }
        return Vector3.zero;
    }
}