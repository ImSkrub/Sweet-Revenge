using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BaseGun,IWeapon
{
    private float lastAttackTime;
    [SerializeField] private AudioClip shootSound;
    private void Start()
    {
        base.Start();
        lastAttackTime = Time.time;
    }

    public override void Attack()
    {
        // Use attackSpeed from Parameters to determine the cooldown
        if (Time.time >= lastAttackTime + valueGun.attackSpeed)
        {
            Bullet bullet = bulletPool.Get();
            SetBulletDir(bullet);
            SoundFXManager.instance.PlaySoundFXClip(shootSound, transform, 0.2f);
            lastAttackTime = Time.time; // Update last attack time
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

    public void Equip()
    {

    }
    public void Unequip()
    {

    }
}
