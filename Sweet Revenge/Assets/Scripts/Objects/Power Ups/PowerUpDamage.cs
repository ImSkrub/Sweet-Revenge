using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDamage : MonoBehaviour, IPowerUp
{
    [SerializeField] public string Name { get; set; } = "DamagePowerUp";
    public GameObject bullet;
    public void ApplyPowerUp(GameObject bullet)
    {
        bullet.GetComponent<Bullet>().Damage *= 2;
        this.bullet = bullet;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ApplyPowerUp(this.bullet);
            Destroy(gameObject);
        }
    }
}
