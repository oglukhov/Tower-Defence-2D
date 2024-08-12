using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : MonoBehaviour
{
    public GameObject bulletPrefab; // Bullet prefab
    public Transform firePoint; // Firing point
    public float bulletsPerSecond = 1f; // Number of bullets per second
    public float maxHealth = 20; // Maximum health of the tower

    private float fireCooldown = 0f; // Time until next shot
    private List<GameObject> enemiesInRange = new List<GameObject>(); // List of enemies in range
    public float currentHealth; // Current health of the tower
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth; // Initialize current health
        fireCooldown = 1f / bulletsPerSecond; // Initialize the firing interval
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            // Find the closest enemy in range
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
        // Get the enemy component from the target
        // Get the enemy component from the target
        Enemy enemy = target.GetComponent<Enemy>();
        ExplodingEnemy explodingEnemy = target.GetComponent<ExplodingEnemy>();
        FastEnemy fastEnemy = target.GetComponent<FastEnemy>();
        Destroyer destroyer = target.GetComponent<Destroyer>();
        
        // Check if the enemy is not on fire before firing
        if (enemy != null && !enemy.isOnFire)
        {
            // Create a bullet at the fire point
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Get the bullet script and set the target
            FireBullet bulletScript = bulletInstance.GetComponent<FireBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetTarget(target.transform);
            }
            audioSource.Play();
                
        }
        if (explodingEnemy != null && !explodingEnemy.isOnFire)
        {
            // Create a bullet at the fire point
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Get the bullet script and set the target
            FireBullet bulletScript = bulletInstance.GetComponent<FireBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetTarget(target.transform);
            }
            if(PlayerPrefs.GetInt("music", 0) == 1){
                    audioSource.Play();
                }
        }
        if (fastEnemy != null && !fastEnemy.isOnFire)
        {
            // Create a bullet at the fire point
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Get the bullet script and set the target
            FireBullet bulletScript = bulletInstance.GetComponent<FireBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetTarget(target.transform);
            }
            if(PlayerPrefs.GetInt("music", 0) == 1){
                    audioSource.Play();
                }
        }
        if (destroyer != null && !destroyer.isOnFire)
        {
            // Create a bullet at the fire point
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Get the bullet script and set the target
            FireBullet bulletScript = bulletInstance.GetComponent<FireBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetTarget(target.transform);
            }
            if(PlayerPrefs.GetInt("music", 0) == 1){
                    audioSource.Play();
                }
        }
        
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