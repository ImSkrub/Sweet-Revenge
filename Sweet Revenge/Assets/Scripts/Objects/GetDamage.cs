using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        PlayerLife Jugador = collision.collider.GetComponent<PlayerLife>();
        if (Jugador != null)
        {
            Jugador.GetDamage(damage);
        }
    }
}
