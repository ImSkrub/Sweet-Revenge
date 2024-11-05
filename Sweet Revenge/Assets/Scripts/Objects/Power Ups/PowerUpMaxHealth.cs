using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMaxHealth : MonoBehaviour, IPowerUp
{
    [SerializeField] public string Name { get; set; } = "MaxHealthPowerUp";

    public void ApplyPowerUp(GameObject player)
    {
        player.GetComponent<PlayerLife>().maxHealth = 200;
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
