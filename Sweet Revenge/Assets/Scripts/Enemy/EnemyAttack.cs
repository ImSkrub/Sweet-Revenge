using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private PlayerLife player;
    [SerializeField] private float damage;


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //   if (collision.collider.tag == "Player")
    //    {
    //        Debug.Log("Enemigo colisionó");
    //        player.GetDamage(damage);
    //    }
    //}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Enemigo colisionó");
            player.GetDamage(damage * Time.deltaTime);
        }
    }


}
