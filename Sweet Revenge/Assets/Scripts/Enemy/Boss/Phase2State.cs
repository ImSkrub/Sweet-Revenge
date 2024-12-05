using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Phase2State : IBossState
{
    private Boss boss;
    private float attackRangeMin = 1f;
    private float attackRangeMax = 4f;
    private float knockbackForce = 4f;
    private float damage = 20f;
    private float moveSpeed = 3.5f;
    private float attackCooldown = 1f; // Cooldown duration in seconds
    private float lastAttackTime = 0f; // Time of the last attack

    public void Enter(Boss boss)
    {
        boss.sr.color = Color.red;
        Debug.Log("Entre fase 2");
        this.boss = boss;
    }

    public void Exit()
    {

    }
    public void UpdateState()
    {
        float distanceToPlayer = Vector2.Distance(boss.transform.position, boss.playerTransform.position);
        boss.FollowPlayer(moveSpeed);

        if (distanceToPlayer <= attackRangeMax && distanceToPlayer >= attackRangeMin)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                boss.Attack(damage);
            }
        }
        
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
