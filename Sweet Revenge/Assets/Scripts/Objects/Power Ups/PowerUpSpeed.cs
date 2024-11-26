using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : MonoBehaviour, IPowerUp
{
    [SerializeField] public string Name { get; set; } = "DamagePowerUp";
    public void ApplyPowerUp(GameObject player)
    {
        player.GetComponent<PlayerController>().speed *= 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ApplyPowerUp(collision.gameObject);
            Destroy(gameObject);
        }
    }    
}
