using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAxe : MonoBehaviour
{
public float speed = 5f; // Bullet speed
    private Transform target; // Bullet target
    private Destroyer destroyer;

    private void Start()
    {
        destroyer = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Destroyer>();
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
                tower.TakeDamage(destroyer.attackDamage); // Deal damage to the tower
            }
            if (fireTower != null)
            {
                fireTower.TakeDamage(destroyer.attackDamage);
            }
            if (frozenTower != null)
            {
                frozenTower.TakeDamage(destroyer.attackDamage);
            }
            if(laserTower != null){
                laserTower.TakeDamage(destroyer.attackDamage);
            }

            // Destroy the bullet after hitting the target
            Destroy(gameObject, 1f);
        }
    }
}
