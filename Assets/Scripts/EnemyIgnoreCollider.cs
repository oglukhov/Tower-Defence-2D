using System.Collections.Generic;
using UnityEngine;

public class EnemyIgnoreColliders2D : MonoBehaviour
{
    // Список GameObject-ів, колайдери яких вороги мають ігнорувати
    public List<GameObject> objectsToIgnore;

    void Start()
    {
        IgnoreCollisionsWithObjects();
    }

    void IgnoreCollisionsWithObjects()
    {
        // Отримуємо 2D CircleCollider ворога
        CircleCollider2D enemyCollider = GetComponent<CircleCollider2D>();

        if (enemyCollider == null)
        {
            Debug.LogError("CircleCollider2D не знайдено на ворогу!", this);
            return;
        }

        // Ігноруємо зіткнення з кожним 2D BoxCollider-ом у списку GameObject-ів
        foreach (GameObject obj in objectsToIgnore)
        {
            if (obj != null)
            {
                BoxCollider2D[] colliders = obj.GetComponents<BoxCollider2D>();
                foreach (BoxCollider2D collider in colliders)
                {
                    Debug.Log("Ігнорую зіткнення між " + enemyCollider.name + " та " + collider.name, this);
                    Physics2D.IgnoreCollision(enemyCollider, collider);
                }
            }
            else
            {
                Debug.LogWarning("Знайдено null об'єкт у списку objectsToIgnore", this);
            }
        }
    }
}