using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBat : MonoBehaviour, IWeapons
{
    [SerializeField] private Animation anim;
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;

    float timeUntilAttack;

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
