using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase1State : IBossState
{
    private Boss boss;
    private float attackRangeMin = 0.5f;
    private float attackRangeMax = 3f;
    private float knockbackForce = 3f;
    private float damage = 15f;
    private float moveSpeed = 2f;

    public void Enter(Boss boss)
    {
        this.boss = boss;
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

    public void Exit()
    {
        //anim.set transform
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
