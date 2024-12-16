using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Phase2State : IBossState
{
    private Boss boss;
    private float attackRangeMin = 1f;
    private float attackRangeMax = 10f;
    private float knockbackForce = 1f;
    private float damage = 20f;
    private float moveSpeed = 5f;
    private float attackCooldown = 1f; // Cooldown duration in seconds
    private float lastAttackTime = 0f; // Time of the last attack
    private float scaleVelocity = 1f;

    public void Enter(Boss boss)
    {
        this.boss = boss;
        boss.sr.color = Color.magenta;
        Debug.Log("Entre fase 2");
    }

    public void Exit()
    {

    }
    public void UpdateState()
    {
        scaleVelocity += scaleVelocity * Time.deltaTime;
        if (scaleVelocity > 2f) scaleVelocity = 2f;
        float distanceToPlayer = Vector2.Distance(boss.transform.position, boss.playerTransform.position);
        boss.FollowPlayer(moveSpeed);
        boss.transform.localScale = new Vector3 (scaleVelocity, scaleVelocity,2) ;

        if (distanceToPlayer <= attackRangeMax && distanceToPlayer >= attackRangeMin)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                boss.Attack(damage);
                Debug.Log("attack phase2");
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
