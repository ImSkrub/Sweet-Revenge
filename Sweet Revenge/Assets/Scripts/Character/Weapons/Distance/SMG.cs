using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : BaseGun,IWeapon
{
    private float lastAttackTime;
    [SerializeField] private const float bulletsPerSecond = 15f; // 15 bullets per second
    [SerializeField] private float slowDownFactor = 0.5f; // Adjust as needed
    private PlayerController playerController;

    private void Start()
    {
        base.Start();
        lastAttackTime = Time.time;
        playerController = FindObjectOfType<PlayerController>(); // Find the player controller in the scene
    }

    public override void Attack()
    {
        float timeBetweenShots = 1f / bulletsPerSecond;

        if (Time.time >= lastAttackTime + timeBetweenShots)
        {
            ShootBullet();
            lastAttackTime = Time.time; // Update last attack time
        }
    }

    private void ShootBullet()
    {
        Bullet bullet = bulletPool.Get();
        if (bullet != null)
        {
            ApplyKnockback(bullet);
        }
    }

    private void ApplyKnockback(Bullet bullet)
    {
        bullet.ApplyKnockback(valueGun.knockbackForce);
    }

    public void Equip()
    {
        if (playerController != null)
        {
            playerController.MovementSpeed *= slowDownFactor; // Slow down the player
        }
    }

    public void Unequip()
    {
        if (playerController != null)
        {
            playerController.MovementSpeed /= slowDownFactor; // Restore original speed
        }
    }
}

