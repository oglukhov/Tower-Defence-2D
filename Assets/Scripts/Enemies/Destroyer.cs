using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Destroyer : MonoBehaviour
{
    public float speed = 3f; // Speed of the enemy
    public float stopDistance = 1f; // Distance to stop from the tower
    public float maxHealth = 10f; // Maximum health of the enemy
    public float attackDamage = 1f; // Damage dealt to the tower
    public float attackInterval = 1f; // Time between attacks
    public float price = 25f; // Price of the enemy when killed
    EnemyTracker enemyTracker;

    public float currentHealth; // Current health of the enemy
    private GameObject currentTarget;
    private bool isTargetingTower = false;
    private Transform towerTransform;
    private bool isAttacking = false;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public bool isOnFire = false; // Instance variable to track if the enemy is on fire
    public bool isOnIce = false; // Instance variable to track if the enemy is frozen

    public ParticleSystem fireEffect;
    public ParticleSystem iceEffect; // Particle system for visual ice effect
    public ParticleSystem bloodEffect; // Particle system for visual blood effect

    private Coroutine fireCoroutine; // Coroutine to handle the fire state
    private Coroutine iceCoroutine; // Coroutine to handle the ice state

    private TowerManager towerManager;
    private AudioSource audioSource;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth; // Initialize current health
        StartCoroutine(SpawnBullets());

        // Find the TowerManager instance
        towerManager = FindObjectOfType<TowerManager>();
        enemyTracker = GameObject.Find("LevelManager").GetComponent<EnemyTracker>();
    }

    void Update()
    {
        if (currentTarget == null && !isTargetingTower)
        {
            FindNearestDiamond();
        }

        MoveTowardsTarget();
        if(ToggleButtonImage.musicOn){
            audioSource.volume = 1;
        }
        if(!ToggleButtonImage.musicOn){
            audioSource.volume = 0;
        }
    }

    void FindNearestDiamond()
    {
        GameObject[] diamonds = GameObject.FindGameObjectsWithTag("Diamond");
        GameObject nearestDiamond = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject diamond in diamonds)
        {
            float distance = Vector3.Distance(transform.position, diamond.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestDiamond = diamond;
            }
        }

        currentTarget = nearestDiamond;
    }

    void MoveTowardsTarget()
    {
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        else if (isTargetingTower && towerTransform != null)
        {
            float distanceToTower = Vector3.Distance(transform.position, towerTransform.position);
            if (distanceToTower > stopDistance)
            {
                Vector3 direction = (towerTransform.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
            else if (!isAttacking)
            {
                StartCoroutine(AttackTower());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            currentTarget = null;
            isTargetingTower = true;
            towerTransform = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            isTargetingTower = false;
            towerTransform = null;
            FindNearestDiamond();
        }
    }

    private IEnumerator AttackTower()
    {
        isAttacking = true;

        while (isTargetingTower && towerTransform != null)
        {
            float distanceToTower = Vector3.Distance(transform.position, towerTransform.position);
            if (distanceToTower <= stopDistance)
            {
                // Instantiate bullet at firePoint position and rotation
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                EnemyAxe bulletScript = bullet.GetComponent<EnemyAxe>();
                if (bulletScript != null)
                {
                    // Set the target for the bullet
                    bulletScript.SetTarget(towerTransform);
                    audioSource.Play();
                }
                
                // Wait for next attack interval
                yield return new WaitForSeconds(attackInterval);
            }
            else
            {
                isAttacking = false;
                yield break;
            }
        }

        isAttacking = false;
    }

    private IEnumerator SpawnBullets()
    {
        while (true)
        {
            if (isTargetingTower && towerTransform != null && !isAttacking)
            {
                StartCoroutine(AttackTower());
            }

            yield return null;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        ActivateBloodEffect();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamageOverTime(int damagePerSecond, float duration)
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
        fireCoroutine = StartCoroutine(DamageOverTime(damagePerSecond, duration));
    }

    private IEnumerator DamageOverTime(int damagePerSecond, float duration)
    {
        isOnFire = true;
        ActivateFireEffect();
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            TakeDamage(damagePerSecond);
            elapsedTime += 1f;
            yield return new WaitForSeconds(1f);
        }
        isOnFire = false;
        if (fireEffect != null)
        {
            fireEffect.Stop();
        }
    }

    public void ActivateFireEffect()
    {
        if (fireEffect != null)
        {
            fireEffect.Play();
        }
    }

    public void ActivateBloodEffect()
    {
        if (bloodEffect != null)
        {
            bloodEffect.Play();
        }
    }

    public void Freeze(float duration)
    {
        if (iceCoroutine != null)
        {
            StopCoroutine(iceCoroutine);
        }
        iceCoroutine = StartCoroutine(FreezeEffect(duration));
    }

    private IEnumerator FreezeEffect(float duration)
    {
        isOnIce = true;
        ActivateIceEffect();
        float originalSpeed = speed;
        float originalAttackInterval = attackInterval;

        speed /= 2;
        attackInterval *= 2;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        attackInterval = originalAttackInterval;
        isOnIce = false;
        if (iceEffect != null)
        {
            iceEffect.Stop();
        }
    }

    public void ActivateIceEffect()
    {
        if (iceEffect != null)
        {
            iceEffect.Play();
        }
    }

    void Die()
    {
        // Handle enemy death (e.g., play animation, drop loot, etc.)
        if (towerManager != null)
        {
            towerManager.AddCoins((int)price); // Add the enemy's price to the TowerManager's coin balance
        }
        EnemySpawner enemySpawner = GameObject.Find("LevelManager").GetComponent<EnemySpawner>();
        enemySpawner.enemiesAlive--;
        enemyTracker.destroyerKilledTimes++;
        Destroy(gameObject);
    }
}
