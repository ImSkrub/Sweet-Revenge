using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpikeBat : MonoBehaviour, IWeapon
{
    [Header("Parameters")]
    [SerializeField] private Parameters dataBat;
    [SerializeField] private LayerMask enemyLayer;
    public string Name { get; private set; } = "SpikeBat";
    private Animation anim;
    private float lastAttackTime;
    private PlayerController player;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        player = FindAnyObjectByType<PlayerController>();
    }

    public void Attack()
    {
        if (Time.time >= lastAttackTime + 1f / dataBat.attackSpeed)
        {
            //Even if you dont find enemy you deduct stamina
            player.Stamina -= dataBat.attackCost; // Deduct stamina
            if (player.Stamina < 0) player.Stamina = 0;
            player.UpdateStaminaBar();
            // Log the position and attack range
            //Debug.Log($"Attack Position: {transform.position}, Attack Range: {dataBat.attackRange}");

            // Detect enemies within attack range
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, dataBat.attackRange, enemyLayer);

            if (hitEnemies.Length > 0)
            {
                //Debug.Log($"Detected {hitEnemies.Length} enemies within range.");
                foreach (Collider2D enemy in hitEnemies)
                {
                    //Debug.Log($"Attacking enemy: {enemy.name} at position: {enemy.transform.position}");
                    ApplyDamage(enemy);
                    ApplyKnockback(enemy);
                }
            }
            else
            {
                //Debug.Log("No enemies detected within attack range.");
            }

            lastAttackTime = Time.time;
        }
        else
        {
            //Debug.Log($"{Name} is on cooldown. Time until next attack: {lastAttackTime + 1f / dataBat.attackSpeed - Time.time}");
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
        else
        {
            //Debug.Log($"{enemy.name} is not damageable.");
        }
    }

    private void ApplyKnockback(Collider2D enemy)
    {
        // Get the NavMeshAgent component from the enemy
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            // Calculate the knockback direction
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;

            // Set the velocity of the NavMeshAgent to create a knockback effect
            // You can adjust the knockback force as needed
            float knockbackForce = dataBat.knockbackForce; // Assuming dataBat is accessible here
            Vector3 knockbackVelocity = knockbackDirection * knockbackForce;

            // Apply the knockback by setting the agent's velocity
            navMeshAgent.velocity = new Vector3(knockbackVelocity.x, knockbackVelocity.y, 0);

            // Optionally, you can stop the agent from moving towards the target for a short duration
            navMeshAgent.isStopped = true;

            // Optionally, you can use a coroutine to resume the agent's movement after a delay
            StartCoroutine(ResumeNavMeshAgent(navMeshAgent, 0.5f)); // Adjust the delay as needed
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
            else
            {
                // Debug.Log($"{enemy.name} does not have a Rigidbody2D or NavMeshAgent component.");
            }
        }
    }

    // Coroutine to resume the NavMeshAgent's movement after a delay
    private IEnumerator ResumeNavMeshAgent(NavMeshAgent agent, float delay)
    {
        yield return new WaitForSeconds(delay);
        agent.isStopped = false; // Resume movement
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
