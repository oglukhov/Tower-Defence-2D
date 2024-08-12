using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float separationRadius = 1.0f;
    public float separationForce = 1.0f;

    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        // Знайти всіх ворогів на сцені і додати їх до списку
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in foundEnemies)
        {
            enemies.Add(enemy);
        }
    }

    void Update()
    {
        foreach (GameObject enemy in enemies)
        {
            ApplySeparation(enemy);
        }
    }

    void ApplySeparation(GameObject enemy)
    {
        Vector3 separationVector = Vector3.zero;
        int count = 0;

        foreach (GameObject otherEnemy in enemies)
        {
            if (otherEnemy != enemy)
            {
                float distance = Vector3.Distance(enemy.transform.position, otherEnemy.transform.position);
                if (distance < separationRadius)
                {
                    Vector3 direction = enemy.transform.position - otherEnemy.transform.position;
                    direction.Normalize();
                    separationVector += direction / distance;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            separationVector /= count;
            separationVector.Normalize();
            separationVector *= separationForce;
            enemy.transform.position += separationVector * Time.deltaTime;
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
}