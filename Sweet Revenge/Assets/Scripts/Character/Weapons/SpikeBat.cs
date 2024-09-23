using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBat : MonoBehaviour, IWeapons
{
    [Header("Parameters")]
    [SerializeField] private Animation anim;
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float knockback = 10f;
    [SerializeField][Range(0,1)] private float timeUntilAttack;

    private void Update()
    {
        Attack();
    }

    public void Attack()
    {
        if (timeUntilAttack <= 0f)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //anim.SetTrigger("Attack");
                timeUntilAttack = attackSpeed;
            }
        }
        else
        {
            timeUntilAttack -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            //collision.collider.GetComponent<Enemy>().TakeDamage(damage);
            Debug.Log("Enemy hit");
        }
    }
}
