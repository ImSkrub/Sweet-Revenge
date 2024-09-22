using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IWeapons
{
    Vector3 targetRotation;
    public Transform pistol;
    public int bulletSpeed;
    public GameObject bullet;
    Vector3 target;
    public SpriteRenderer pistolSR;
    [SerializeField] PlayerController player;
    [SerializeField] public float pistolDamage;

    public bool canRotate = true;

    void Update()
    {
        targetRotation = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(targetRotation.y, targetRotation.x) * Mathf.Rad2Deg;
        if (canRotate)
        {
        pistol.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        if (angle > 90 || angle < -90)
        {
            pistolSR.flipY = true;
        }
        else
        {
            pistolSR.flipY = false;
        }
        }



        if (Input.GetKeyDown(KeyCode.Mouse0) && (player.Stamina > 0))
        {
            if (player.recharge != null) StopCoroutine(player.recharge);
            player.recharge = StartCoroutine(player.RechargeStamina());
            Attack();
        }
    }


    public void Attack()
    {
        var Bullet = Instantiate(bullet, pistol.position, transform.rotation);
        targetRotation.z = 0;
        target = (targetRotation - transform.position).normalized;
        Bullet.GetComponent<Rigidbody2D>().AddForce(target * bulletSpeed, ForceMode2D.Impulse);

        player.Stamina -= player.AttackCost;
        if (player.Stamina < 0) player.Stamina = 0;
        player.StaminaBar.fillAmount = player.Stamina / player.MaxStamina;
    }

}
