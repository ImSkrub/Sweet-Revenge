using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpLife : MonoBehaviour,IPowerUp
{
    [SerializeField] public string Name { get; set; } = "LifeUp";
   
    public void ApplyPowerUp(GameObject player)
    {
        player.GetComponent<PlayerLife>().CurrentHealth += 50;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyPowerUp(collision.gameObject);
        }
    }
}
