using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExplodingEnemy : MonoBehaviour
{
    public float speed = 3f; // Speed of the enemy
    public float stopDistance = 1f; // Distance to stop from the tower
    public float maxHealth = 10; // Maximum health of the enemy
    public float attackDamage = 5; // Damage dealt to the tower
    public float attackInterval = 1f; // Time between attacks
    public float price = 35f;
    EnemyTracker enemyTracker;

    public float currentHealth; // Current health of the enemy
    private GameObject currentTarget;
    private bool isTargetingTower = false;
    private Transform towerTransform;
    public GameObject bigExplosion, smallExplosion;
    public Transform firePoint;
    public bool isOnFire = false; // Instance variable to track if the enemy is on fire
    public bool isOnIce = false; // Instance variable to track if the enemy is frozen

    public ParticleSystem fireEffect;
    public ParticleSystem iceEffect; // Particle system for visual ice effect
    public ParticleSystem bloodEffect; // Particle system for visual blood effect

    private Coroutine fireCoroutine; // Coroutine to handle the fire state
    private Coroutine iceCoroutine; // Coroutine to handle the ice state
    private bool hasExploded = false; // Flag to track if the enemy has already exploded

    private TowerManager towerManager;
    private AudioSource audioSource;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth; // Initialize current health
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
            else
            {
                Explode();
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

    private void Explode()
    {
        if (hasExploded) return; // Prevent multiple explosions
        hasExploded = true;

        // Instantiate big explosion effect
        if (bigExplosion != null)
        {
            Instantiate(bigExplosion, transform.position, Quaternion.identity);
        }

        // Deal damage to the tower
        if (towerTransform != null)
        {
            Tower tower = towerTransform.GetComponentInParent<Tower>();
            FireTower fireTower = towerTransform.GetComponentInParent<FireTower>();
            FrozenTower frozenTower = towerTransform.GetComponentInParent<FrozenTower>();
            LaserTower laserTower = towerTransform.GetComponentInParent<LaserTower>();

            if (tower != null)
            {
                tower.TakeDamage(attackDamage);
            }
            if (fireTower != null)
            {
                fireTower.TakeDamage(attackDamage);
            }
            if (frozenTower != null)
            {
                frozenTower.TakeDamage(attackDamage);
            }
            if (laserTower != null)
            {
                laserTower.TakeDamage(attackDamage);
            }
        }
        EnemySpawner enemySpawner = GameObject.Find("LevelManager").GetComponent<EnemySpawner>();
        enemySpawner.enemiesAlive--;
        audioSource.Play();
        // Destroy the enemy after explosion
        Destroy(gameObject, 0.4f);
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

    private void Die()
    {
        if (hasExploded) return; // Prevent multiple explosions
        hasExploded = true;

        // Instantiate small explosion effect
        if (smallExplosion != null)
        {
            Instantiate(smallExplosion, transform.position, Quaternion.identity);
        }

        if (towerManager != null)
        {
            towerManager.AddCoins((int)price);
        }

        EnemySpawner enemySpawner = GameObject.Find("LevelManager").GetComponent<EnemySpawner>();
        enemySpawner.enemiesAlive--;
        enemyTracker.demomanKilledTimes++;
        // Destroy the enemy after small explosion
        audioSource.Play();
        Destroy(gameObject, 0.4f);
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
}