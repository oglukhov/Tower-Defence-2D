using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenTower : MonoBehaviour
{
    public GameObject bulletPrefab; // Bullet prefab
    public Transform firePoint; // Firing point
    public float bulletsPerSecond = 1f; // Number of bullets per second
    public float maxHealth = 20; // Maximum health of the tower

    private float fireCooldown = 0f; // Time until next shot
    private List<GameObject> enemiesInRange = new List<GameObject>(); // List of enemies in range
    private Dictionary<GameObject, GameObject> enemyBulletMap = new Dictionary<GameObject, GameObject>(); // Map of enemies to bullets
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
            FireAtEnemies();
            fireCooldown = 1f / bulletsPerSecond;
        }
        if (ToggleButtonImage.musicOn)
        {
            audioSource.volume = 1;
        }
        if (!ToggleButtonImage.musicOn)
        {
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
            if (enemyBulletMap.ContainsKey(other.gameObject))
            {
                Destroy(enemyBulletMap[other.gameObject]);
                enemyBulletMap.Remove(other.gameObject);
            }
        }
    }

    void FireAtEnemies()
    {
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                Fire(enemy);
            }
        }
    }

    void Fire(GameObject target)
    {
        // Check if the enemy is not on ice before firing
        if (target.TryGetComponent(out Enemy enemy) && !enemy.isOnIce)
        {
            CreateBullet(target);
        }
        else if (target.TryGetComponent(out ExplodingEnemy explodingEnemy) && !explodingEnemy.isOnIce)
        {
            CreateBullet(target);
        }
        else if (target.TryGetComponent(out FastEnemy fastEnemy) && !fastEnemy.isOnIce)
        {
            CreateBullet(target);
        }
        else if (target.TryGetComponent(out Destroyer destroyer) && !destroyer.isOnIce)
        {
            CreateBullet(target);
        }
    }

    void CreateBullet(GameObject target)
    {
        // If a bullet already exists for the target and the target is not frozen, destroy the existing bullet
        if (enemyBulletMap.ContainsKey(target) && enemyBulletMap[target] != null)
        {
            Destroy(enemyBulletMap[target]);
            enemyBulletMap.Remove(target);
        }

        // If the target is frozen, do not create a new bullet
        if (target.TryGetComponent(out Enemy enemy) && enemy.isOnIce)
        {
            return;
        }
        else if (target.TryGetComponent(out ExplodingEnemy explodingEnemy) && explodingEnemy.isOnIce)
        {
            return;
        }
        else if (target.TryGetComponent(out FastEnemy fastEnemy) && fastEnemy.isOnIce)
        {
            return;
        }
        else if (target.TryGetComponent(out Destroyer destroyer) && destroyer.isOnIce)
        {
            return;
        }

        // Create a bullet at the fire point
        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Get the bullet script and set the target
        FrozenBullet bulletScript = bulletInstance.GetComponent<FrozenBullet>();
        if (bulletScript != null)
        {
            audioSource.Play();
            bulletScript.SetTarget(target.transform);

            // Add the bullet to the map
            enemyBulletMap[target] = bulletInstance;
        }
        else
        {
            Destroy(bulletInstance); // Destroy the bullet if the script is missing
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
        if (VibrationToggle.isVibrationOn)
        {
            Handheld.Vibrate();
        }

        // Handle the tower destruction (e.g., animation, removal from the game, etc.)
        Destroy(gameObject, 0.2f);
    }
}