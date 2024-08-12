using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float speed = 5f; // Speed of the bullet
    private Transform target; // Target of the bullet
    public float burnDuration = 3f; // Burn duration
    public int damagePerSecond = 1; // Damage per second

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Rotate the arrow to face the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Destroy the bullet if it reaches the target or is close to it
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Start dealing damage over time
            ExplodingEnemy explodingEnemy = target.GetComponent<ExplodingEnemy>();  
            Enemy enemy = target.GetComponent<Enemy>();
            FastEnemy fastEnemy = target.GetComponent<FastEnemy>();
            Destroyer destroyer = target.GetComponent<Destroyer>();
            if (enemy != null)
            {
                enemy.TakeDamageOverTime(damagePerSecond, burnDuration);
            }
            if(explodingEnemy != null){
                explodingEnemy.TakeDamageOverTime(damagePerSecond, burnDuration);
            }
            if(fastEnemy != null){
                fastEnemy.TakeDamageOverTime(damagePerSecond, burnDuration);
            }
            if(destroyer != null){
                destroyer.TakeDamageOverTime(damagePerSecond, burnDuration);
            }
            Destroy(gameObject);
        }
    }
}