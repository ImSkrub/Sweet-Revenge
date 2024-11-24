using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2State : MonoBehaviour,IBossState
{
    private Boss boss;
    private float attackRangeMin = 1f;
    private float attackRangeMax = 5f;
    private float knockbackForce = 5f;
    private float damage = 30f;
    private float moveSpeed = 3f;

    public void Enter(Boss boss)
    {
        this.boss = boss;
    }

    public void Exit()
    {

    }
    public void UpdateState()
    {
        float distanceToPlayer = Vector2.Distance(boss.transform.position, boss.jugador.position);
        boss.FollowPlayer(moveSpeed);

        if (distanceToPlayer <= attackRangeMax && distanceToPlayer >= attackRangeMin)
        {
            boss.Attack(damage);
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
