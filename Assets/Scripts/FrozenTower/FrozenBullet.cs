using UnityEngine;

public class FrozenBullet : MonoBehaviour
{
    public float speed = 5f; // Speed of the bullet
    private Transform target; // Target of the bullet
    public float freezeDuration = 2f; // Freeze duration
    public int damage = 1; // Damage dealt by the bullet

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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Destroy the bullet if it reaches the target or is close to it
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Apply freeze effect and damage
            Enemy enemy = target.GetComponent<Enemy>();
            ExplodingEnemy explodingEnemy = target.GetComponent<ExplodingEnemy>();
            FastEnemy fastEnemy = target.GetComponent<FastEnemy>();
            Destroyer destroyer = target.GetComponent<Destroyer>();
            if (enemy != null)
            {
                enemy.Freeze(freezeDuration);
                enemy.TakeDamage(damage); // Apply damage to the enemy
            }
            if(explodingEnemy != null){
                explodingEnemy.Freeze(freezeDuration);
                explodingEnemy.TakeDamage(damage);
            }
            if(fastEnemy != null){
                fastEnemy.Freeze(freezeDuration);
                fastEnemy.TakeDamage(damage);
            }
            if(destroyer != null){
                destroyer.Freeze(freezeDuration);
                destroyer.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}