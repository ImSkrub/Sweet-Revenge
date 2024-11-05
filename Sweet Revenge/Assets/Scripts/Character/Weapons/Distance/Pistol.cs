using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BaseGun,IWeapon
{
    private float lastAttackTime;
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
            bulletPool.Get();
            lastAttackTime = Time.time; // Update last attack time
        }
    }
}
