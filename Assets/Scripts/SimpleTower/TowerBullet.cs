using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    public float speed = 5f; // Швидкість кулі
    public float damage = 1;
    private Transform target;

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Рух до цілі
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Знищити кулю, якщо вона досягла цілі або близько до неї
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Завдати шкоди ворогу
            ExplodingEnemy explodingEnemy = target.GetComponent<ExplodingEnemy>();
            Enemy enemy = target.GetComponent<Enemy>();
            FastEnemy fastEnemy = target.GetComponent<FastEnemy>(); 
            Destroyer destroyer = target.GetComponent<Destroyer>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Припустимо, що шкода від кулі дорівнює 1
            }
            if(explodingEnemy != null){
                explodingEnemy.TakeDamage(damage);
            }
            if(fastEnemy != null){
                fastEnemy.TakeDamage(damage);
            }
            if(destroyer != null){
                destroyer.TakeDamage(damage);
            }

            // Знищити кулю після влучення
            Destroy(gameObject);
        }
    }
}