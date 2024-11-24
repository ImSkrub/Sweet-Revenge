using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPowerUp : MonoBehaviour, IPowerUp
{
    [SerializeField] public string Name { get; set; } = "StaminaPowerUp";
    public void ApplyPowerUp(GameObject player)
    {
        player.GetComponent<PlayerController>().staminaRechargeRate *= 3;
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
