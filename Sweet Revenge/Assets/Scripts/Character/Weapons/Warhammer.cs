using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Warhammer : MonoBehaviour,IWeapon
{
    [Header("Parameters")]
    [SerializeField] private Parameters dataHammer;
    [SerializeField] private LayerMask enemyLayer;
    public string Name { get; private set; } = "Warhammer";
    private Animation anim;
    private float lastAttackTime;
    private float specialAttackCooldown;
    [SerializeField] private float slowDownDuration = 5f; // Duration of the slowdown effect
    [SerializeField] private float specialAttackCooldownTime = 5f; // Cooldown for the special attack
    private PlayerController player;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        player = FindAnyObjectByType<PlayerController>();
    }

    public void Attack()
    {
        if (Time.time >= lastAttackTime + 1f / dataHammer.attackSpeed)
        {
            // Deduct stamina
            player.Stamina -= dataHammer.attackCost;
            if (player.Stamina < 0) player.Stamina = 0;
            player.UpdateStaminaBar();

            // Detect enemies within attack range
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, dataHammer.attackRange, enemyLayer);

            if (hitEnemies.Length > 0)
            {
                foreach (Collider2D enemy in hitEnemies)
                {
                    ApplyDamage(enemy);
                    ApplyKnockback(enemy);
                    if (Input.GetKeyDown(KeyCode.E) && Time.time >= specialAttackCooldown)
                    {
                        SpecialAttack();
                    }
                }
            }

            lastAttackTime = Time.time;
        }
        else
        {
            Debug.Log($"{Name} is on cooldown. Time until next attack: {lastAttackTime + 1f / dataHammer.attackSpeed - Time.time}");
        }
    }

    private void SpecialAttack()
    {
        // Calculate the area of effect for the special attack
        float specialAttackRange = dataHammer.attackRange * 2f; // Double the normal range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, specialAttackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(dataHammer.damage * 2); // Double damage
                ApplyKnockback(enemy, true); // Apply increased knockback
            }
        }

        // Apply player slowdown effect
        StartCoroutine(ApplySlowdown());

        // Set the cooldown for the special attack
        specialAttackCooldown = Time.time + specialAttackCooldownTime;
    }

    private IEnumerator ApplySlowdown()
    {
        float originalSpeed = player.MovementSpeed; // Assuming you have a MovementSpeed property
        player.MovementSpeed *= 0.5f; // Slow down the player by 50%

        yield return new WaitForSeconds(slowDownDuration); // Wait for the duration of the slowdown

        player.MovementSpeed = originalSpeed; // Reset the player's speed
    }

    private void ApplyDamage(Collider2D enemy)
    {
        IDamageable damageable = enemy.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(dataHammer.damage);
        }
    }

    private void ApplyKnockback(Collider2D enemy, bool isSpecialAttack = false)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            float knockbackForce = isSpecialAttack ? dataHammer.knockbackForce * 2 : dataHammer.knockbackForce; // Double knockback force for special attack
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    // Gizmos to see the range from inspector
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dataHammer.attackRange);
    }
}
