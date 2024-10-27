using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged: MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float rangeToAttack = 10f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform target;
    [SerializeField] private float damage = 25f;

    [Header("References")]
    private PlayerLife playerHealth;


    //Timers
    private float cooldownTimer;
    private float currentTime;

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        currentTime += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                Attack();
            }

        }
    }


    private bool PlayerInSight()
    {
        //direction to player
        Vector2 directionToPlayer = (target.position - transform.position).normalized;
        //Cast a ray from enemy to player. Make sure player has PlayerLife script
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, rangeToAttack, playerLayer);

        //drawing the ray in scene.
        Debug.DrawRay(transform.position, directionToPlayer * rangeToAttack, Color.red);


        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            playerHealth = hit.transform.GetComponent<PlayerLife>();
            return true;
        }
        return false;
    }

    public void Attack()
    {
        if (PlayerInSight())
        {
            //hacer la logica del ranged attack
            playerHealth.GetDamage(damage);
        }
    }


}
