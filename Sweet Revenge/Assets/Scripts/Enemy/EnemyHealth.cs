using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Pistol pistol;
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("bullet"))
        {
            health -= pistol.pistolDamage;
        }
    }

    private void Death()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Death();

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
