using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FastEnemy : MonoBehaviour
{
    public float speed = 4f; // Speed of the fast enemy
    public float maxHealth = 5; // Maximum health of the fast enemy
    public float currentHealth; // Current health of the enemy
    public float price = 35f;
    EnemyTracker enemyTracker;

    private GameObject currentTarget;
    public bool isOnFire = false; // Instance variable to track if the enemy is on fire
    public bool isOnIce = false; // Instance variable to track if the enemy is frozen

    public ParticleSystem fireEffect;
    public ParticleSystem iceEffect; // Particle system for visual ice effect
    public ParticleSystem bloodEffect; // Particle system for visual blood effect

    private Coroutine fireCoroutine; // Coroutine to handle the fire state
    private Coroutine iceCoroutine; // Coroutine to handle the ice state

    private TowerManager towerManager;

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health
        FindNearestDiamond();
        towerManager = FindObjectOfType<TowerManager>();
        enemyTracker = GameObject.Find("LevelManager").GetComponent<EnemyTracker>();
    }

    void Update()
    {
        MoveTowardsTarget();
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
        else
        {
            FindNearestDiamond();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            Vector3 avoidanceDirection = (transform.position - collision.transform.position).normalized;
            Vector3 newDirection = (direction + avoidanceDirection).normalized;
            transform.position += newDirection * speed * Time.deltaTime;
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

        speed /= 2;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
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
        if (towerManager != null)
        {
            towerManager.AddCoins((int)price);
        }
        EnemySpawner enemySpawner = GameObject.Find("LevelManager").GetComponent<EnemySpawner>();
        enemySpawner.enemiesAlive--;
        enemyTracker.dodgerKilledTimes++;
        Destroy(gameObject);
    }
}