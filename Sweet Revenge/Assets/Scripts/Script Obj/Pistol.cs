using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    Vector3 targetRotation;
    public Transform pistol;
    public int bulletSpeed;
    public GameObject bullet;
    Vector3 target;
    public SpriteRenderer armaSR;
    [SerializeField] private CharacterController characterMovement;


    void Update()
    {
        targetRotation = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(targetRotation.y, targetRotation.x) * Mathf.Rad2Deg;
        pistol.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (angle > 90 || angle < -90)
        {
            armaSR.flipY = true;
        }
        else
        {
            armaSR.flipY = false;
        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            disparar();
        }
    }


    void disparar()
    {
        var Bullet = Instantiate(bullet, pistol.position, transform.rotation);
        targetRotation.z = 0;
        target = (targetRotation - transform.position).normalized;
        Bullet.GetComponent<Rigidbody2D>().AddForce(target * bulletSpeed, ForceMode2D.Impulse);
    }

}
