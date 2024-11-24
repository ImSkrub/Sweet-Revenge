using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Space(3)]
    [Header("Weapon")]
    [SerializeField] private Transform weaponSlot;
    private float attackDelay = 0.5f;
    private float lastAttackTime;
    [Header("UI")]
        
    private IWeapon weapon;
    private PlayerController player;
    private PlayerLife lifePlayer;
  
    public event Action Equipped;
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        lifePlayer = GetComponent<PlayerLife>();
    }
    private void Update()
    {
        // Check if the player can attack
        if (Input.GetButton("Fire1") && player.Stamina >= 0 && Time.time >= lastAttackTime + attackDelay)
        {
            player.isRecharging= false;
            weapon?.Attack();
            lastAttackTime = Time.time; // Update the last attack time            
        }
    }

    public void SetNewMaxHealth(int newHealth)
    {
        lifePlayer.maxHealth += newHealth;
        lifePlayer.CurrentHealth = lifePlayer.maxHealth;
    }

    public void SetNewMaxDamage(float newDamage)
    {

    }

    public void SetNewMaxStamina(float newStamina)
    {
        player.SetNewStamina(newStamina);
    }

    public void SetWeapon(IWeapon weapon)
    {
        this.weapon = weapon;
    }

    public void ApplyEffectPowerUp(IPowerUp powerUp)
    {
        if (powerUp != null)
        {
            powerUp.ApplyPowerUp(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            if (weapon != null)
            {
                Destroy(weaponSlot.GetChild(0).gameObject);
            }
            
            SetWeapon(collision.gameObject.GetComponent<IWeapon>());
            collision.gameObject.transform.SetParent(weaponSlot);
            collision.gameObject.transform.position = weaponSlot.position;
            collision.gameObject.transform.rotation = weaponSlot.rotation;
        }

        if (collision.gameObject.CompareTag("PowerUp"))
        {
            ApplyEffectPowerUp(collision.gameObject.GetComponent<IPowerUp>());
            
        }
        
    }
}
