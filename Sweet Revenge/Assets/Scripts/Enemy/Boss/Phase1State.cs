using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase1State : IBossState
{
    private Boss boss;
    private float attackRangeMin = 0.5f;
    private float attackRangeMax = 6f;
    private float knockbackForce = 1f;
    private float damage = 15f;
    private float moveSpeed = 7f;
    private bool attacked = false;
    private float attackCooldown = 1f; // Cooldown duration in seconds
    private float lastAttackTime = 0f; // Time of the last attack

    public void Enter(Boss boss)
    {
        this.boss = boss;
    }

    public void UpdateState()
    {
        float distanceToPlayer = Vector2.Distance(boss.transform.position, boss.playerTransform.position);
        boss.FollowPlayer(moveSpeed);

        // Check if the boss can attack
        if (distanceToPlayer <= attackRangeMax && distanceToPlayer >= attackRangeMin)
        {
            // Check if the cooldown has passed
            if (!attacked && Time.time >= lastAttackTime + attackCooldown)
            {
                //Debug.Log("Ataque");
                boss.Attack(damage);
                attacked = true;
                lastAttackTime = Time.time; // Update the last attack time
            }
        }
        else
        {
            // Reset the attacked flag if the player is out of range
            attacked = false;
        }
    }

    public void Exit()
    {
        Debug.Log("Sali");
    }

    public float GetKnockbackForce()
    {
        return knockbackForce;
    }

    public float GetRange()
    {
        return attackRangeMax;
    }

    public float GetDamage()
    {
        return damage;
    }
}
