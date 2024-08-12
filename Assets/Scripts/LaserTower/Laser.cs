using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float damage = 1f; // Damage per second of the laser

    void OnTriggerStay2D(Collider2D target)
    {
        if (target.gameObject.CompareTag("Enemy"))
        {
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
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // This method can be used if you want to add any additional logic when an enemy enters the laser's range
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // This method can be used if you want to add any additional logic when an enemy exits the laser's range
        }
    }
}