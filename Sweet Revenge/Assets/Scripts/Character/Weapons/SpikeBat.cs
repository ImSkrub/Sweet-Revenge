using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBat : MonoBehaviour, IWeapon
{
    [Header("Parameters")]
    [SerializeField] private Parameters dataBat;
    [SerializeField] private LayerMask enemyLayer;

    private Animation anim;
    private float lastAttackTime;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public void Attack()
    {
        if (Time.time >= lastAttackTime + 1f / dataBat.attackSpeed)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, dataBat.attackRange, enemyLayer);
            foreach(Collider2D enemy in hitEnemies)
            {
                ApplyDamage(enemy);
            }
            lastAttackTime = Time.time;
        }
    }

    private void ApplyDamage(Collider2D enemy)
    {
        IDamageable damageable = enemy.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(dataBat.damage);
        }
    }
   
    private void ApplyKnockback(Collider2D enemy)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            rb.AddForce(knockbackDirection * dataBat.knockbackForce, ForceMode2D.Impulse);
        }
    }
    //Gizmos to see the range from inspector
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dataBat.attackRange);
    }
}
