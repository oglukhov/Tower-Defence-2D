using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab; // Префаб кулі
    public Transform firePoint; // Точка вогню
    public float bulletsPerSecond = 1f; // Кількість куль на секунду
    public float maxHealth = 20; // Максимальне здоров'я башти

    private float fireCooldown = 0f; // Час до наступного пострілу
    private List<GameObject> enemiesInRange = new List<GameObject>(); // Список ворогів, які знаходяться в радіусі дії башти
    public float currentHealth; // Поточне здоров'я башти
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth; // Ініціалізуємо поточне здоров'я
        fireCooldown = 1f / bulletsPerSecond; // Ініціалізуємо початковий інтервал стрільби
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            // Знаходимо найближчого ворога в радіусі дії башти
            GameObject closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                Fire(closestEnemy);
                fireCooldown = 1f / bulletsPerSecond;
            }
        }

        if(ToggleButtonImage.musicOn){
            audioSource.volume = 1;
        }
        if(!ToggleButtonImage.musicOn){
            audioSource.volume = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    void Fire(GameObject target)
    {
        // Створюємо кулю в точці вогню башти
        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
       

        // Отримуємо скрипт кулі і задаємо ціль для пострілу
        TowerBullet bulletScript = bulletInstance.GetComponent<TowerBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target.transform);
        }
            audioSource.Play();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log("Tower health: " + currentHealth); // Debugging current health

        if (currentHealth <= 0)
        {
            DestroyTower();
        }
    }

    void DestroyTower()
{
    // Trigger the device vibration
    if(VibrationToggle.isVibrationOn){
            Handheld.Vibrate();
        }

    // Handle the tower destruction (e.g., animation, removal from the game, etc.)
    
    Destroy(gameObject, 0.2f);
}
}