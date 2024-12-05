using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpikeBat : MonoBehaviour, IWeapon
{
    [Header("Parameters")]
    [SerializeField] private Parameters dataBat;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private AudioClip batSound;
    public string Name { get; private set; } = "SpikeBat";
    private Animator anim;
    private float lastAttackTime;
    private PlayerController player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = FindAnyObjectByType<PlayerController>();
    }

    public void Attack()
    {
        if (Time.time >= lastAttackTime + 1.5f / dataBat.attackSpeed)
        {
            anim.SetTrigger("Attack");
            SoundFXManager.instance.PlaySoundFXClip(batSound, transform, 0.5f);
            //Even if you dont find enemy you deduct stamina
            player.Stamina -= dataBat.attackCost; // Deduct stamina
            if (player.Stamina < 0) player.Stamina = 0;
            player.UpdateStaminaBar();
           
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, dataBat.attackRange, enemyLayer);

            if (hitEnemies.Length > 0)
            {

                foreach (Collider2D enemy in hitEnemies)
                {
                    if (!IsBlockedByWall(enemy)) // Check if the attack is blocked by a wall
                    {
                        ApplyDamage(enemy);
                        ApplyKnockback(enemy);
                    }
                }
            }
            lastAttackTime = Time.time;
        }
    }

    private void ApplyDamage(Collider2D enemy)
    {
        IDamageable damageable = enemy.GetComponent<IDamageable>();
        if (damageable != null)
        {
            //Debug.Log($"Applying {dataBat.damage} damage to {enemy.name}");
            damageable.TakeDamage(dataBat.damage);
        }
    }

    private void ApplyKnockback(Collider2D enemy)
    {
        // Get the NavMeshAgent component from the enemy
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;

            float knockbackForce = dataBat.knockbackForce; // Assuming dataBat is accessible here
            Vector3 knockbackVelocity = knockbackDirection * knockbackForce;

            // Apply the knockback by setting the agent's velocity
            navMeshAgent.velocity = new Vector3(knockbackVelocity.x, knockbackVelocity.y, 0);

            // Stop the agent from moving towards the target for a short duration
            navMeshAgent.isStopped = true;

            // Start the coroutine to resume the agent's movement after a delay
            StartCoroutine(ResumeNavMeshAgent(navMeshAgent, 0.75f)); // Adjust the delay as needed
        }
        else
        {
            // If the enemy does not have a NavMeshAgent, check for Rigidbody2D as a fallback
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                rb.AddForce(knockbackDirection * dataBat.knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
    // Coroutine to resume the NavMeshAgent's movement after a delay
    private IEnumerator ResumeNavMeshAgent(NavMeshAgent agent, float delay)
    {
        if (agent != null)
        {
            yield return new WaitForSeconds(delay);

            // Check if the agent is still valid before resuming
            if (agent != null)
            {
                agent.isStopped = false; // Resume movement
            }
        }
    }

    private bool IsBlockedByWall(Collider2D enemy)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, enemy.transform.position, LayerMask.GetMask("Wall"));
       

        return hit.collider != null;
    }

    public void Equip()
    {
        Debug.Log("Me equipe");
    }
    public void Unequip()
    {
        Debug.Log("Me desequipe");
    }
    // Gizmos to see the range from inspector
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dataBat.attackRange);
    }
}
