using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : BaseGun,IWeapon
{
    private float lastAttackTime;
    [SerializeField] private const float bulletsPerSecond = 35f; // 15 bullets per second
    [SerializeField] private float slowDownFactor = 0.5f; // Adjust as needed
    private PlayerController playerController;
    [SerializeField] private AudioClip smgSound;

    private void Start()
    {
        base.Start();
        lastAttackTime = Time.time;
        playerController = FindObjectOfType<PlayerController>(); // Find the player controller in the scene
    }

    public override void Attack()
    {
        float timeBetweenShots = 1f / valueGun.attackSpeed +bulletsPerSecond;

        if (Time.time >= lastAttackTime + valueGun.attackSpeed)
        {
            ShootBullet();
            lastAttackTime = Time.time; // Update last attack time
            SoundFXManager.instance.PlaySoundFXClip(smgSound, transform, 0.2f);
        }
    }

    private void ShootBullet()
    {
        Bullet bullet = bulletPool.Get();
        if (bullet != null)
        {
            SetBulletDir(bullet);
            ApplyKnockback(bullet);
        }
    }

    private void SetBulletDir(Bullet bullet)
    {
        bullet.transform.position = transform.position;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0;

        bullet.SetDirection(direction);
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

