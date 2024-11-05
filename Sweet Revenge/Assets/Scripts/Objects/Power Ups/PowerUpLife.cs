using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpLife : MonoBehaviour, IPowerUp
{
    [SerializeField] public string Name { get; set; } = "LifePowerUp";

    public void ApplyPowerUp(GameObject player)
    {
       player.GetComponent<PlayerLife>().CurrentHealth += 50;
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
