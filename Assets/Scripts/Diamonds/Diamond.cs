using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    private bool isCollected = false; // Flag to prevent multiple triggers
    EnemySpawner enemySpawner;

    void Start()
    {
        enemySpawner = GameObject.Find("LevelManager").GetComponent<EnemySpawner>();
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (!isCollected && target.CompareTag("Enemy"))
        {
            isCollected = true; // Set the flag to true to prevent further triggers
            DiamondManager diamondManager = GameObject.Find("DiamondManager").GetComponent<DiamondManager>();
            diamondManager.DecreaseDiamondCount(gameObject); // Pass the diamond object
            enemySpawner.enemiesAlive--;
            Destroy(gameObject);
            Destroy(target.gameObject);
        }
    }
}
