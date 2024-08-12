using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f; // Bullet speed
    private Transform target; // Bullet target
    private Enemy enemy;

    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

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

        // Rotate the bullet to face the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Destroy the bullet if it reaches the target or is close to it
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TowerCenter"))
        {
            // Deal damage to the tower
            Tower tower = other.GetComponentInParent<Tower>();
            FireTower fireTower = other.GetComponentInParent<FireTower>();
            FrozenTower frozenTower = other.GetComponentInParent<FrozenTower>();
            LaserTower laserTower = other.GetComponentInParent<LaserTower>();
            if (tower != null)
            {
                tower.TakeDamage(enemy.attackDamage);
            }
            if (fireTower != null)
            {
                fireTower.TakeDamage(enemy.attackDamage);
            }
            if (frozenTower != null)
            {
                frozenTower.TakeDamage(enemy.attackDamage);
            }
            if(laserTower != null){
                laserTower.TakeDamage(enemy.attackDamage);
            }

            // Destroy the bullet after hitting the target
            Destroy(gameObject, 1f);
        }
    }
}