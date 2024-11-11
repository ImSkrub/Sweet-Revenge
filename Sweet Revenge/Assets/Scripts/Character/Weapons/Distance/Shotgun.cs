using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : BaseGun,IWeapon
{
    private float lastAttackTime;
    private float nextShotTime; // Time when the next shot can be fired
    private int bulletsFired; // Counter for bullets fired in the current attack

    [SerializeField] private int bulletCount = 4; // Number of bullets to shoot
    [SerializeField] private float spreadAngle = 15f; // Spread angle in degrees
    [SerializeField] private float delayBetweenShots = 0.1f; // Delay between each bullet shot

    private void Start()
    {
        base.Start();
        lastAttackTime = Time.time; // Initialize last attack time
        nextShotTime = lastAttackTime; // Initialize next shot time
        
    }

    public override void Attack()
    {
        // Check if enough time has passed since the last attack
        if (Time.time >= lastAttackTime + valueGun.attackSpeed)
        {
            // Reset the bullets fired counter
            bulletsFired = 0;
            lastAttackTime = Time.time; // Update last attack time
            nextShotTime = lastAttackTime; // Set the next shot time to now
        }

        // Fire bullets if we are in the attack window
        if (bulletsFired < bulletCount && Time.time >= nextShotTime)
        {
            bulletPool.Get();
            // Calculate the direction with spread
            //float angle = -spreadAngle + (bulletsFired * (spreadAngle * 2) / (bulletCount - 1));
            //Vector3 direction = Quaternion.Euler(0, 0, angle) * parentTransform.position; // Apply spread angle
            //bullet.SetDirection(direction.normalized); // Set the bullet direction

            // Update the next shot time
            nextShotTime += valueGun.attackSpeed; // Increment the next shot time by the delay
            bulletsFired++; // Increment the bullets fired counter
        }
    }
    public void Equip()
    {

    }
    public void Unequip()
    {

    }
}
