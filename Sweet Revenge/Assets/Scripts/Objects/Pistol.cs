using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Pistol : MonoBehaviour, IWeapon
{
    public string Name { get; private set; } = "Pistol";
    [Header("Parameters")]
    [SerializeField] Parameters valueGun;
    [SerializeField] private Bullet bullet;
    private ObjectPool<Bullet> bulletPool;
    private Transform parentTransform;
    
    private Vector3 targetRotation;
    private Vector3 target;
    private SpriteRenderer pistolSR;
    [SerializeField] private PlayerController player;
 

    //booleans
    public bool canRotate = true;
    public bool canAttack = true;
    public bool pickedUp = false;

    private void Awake()
    {
        bulletPool = new ObjectPool<Bullet>(CreatePoolItem, OnTakeFromPool, OnReturnedFromPool, OnDestroyObject, true, 10, valueGun.maxBullets);
    }
    private void Start()
    {
        pistolSR = GetComponent<SpriteRenderer>();
        parentTransform = GetComponentInParent<Transform>();
    }
    void Update()
    {
        Rotation(); 
    }

    public void Attack()
    {
        if (canAttack && player.Stamina >= valueGun.attackCost)
        {
            Bullet bullet = bulletPool.Get();
            if (bullet != null)
            {
                // Set bullet position and direction
                bullet.SetPosition(transform.position); // Set bullet position to the gun's position
                Vector3 direction = (targetRotation - transform.position).normalized; // Calculate direction
                bullet.SetDirection(direction); // Initialize the bullet with the direction

                player.Stamina -= valueGun.attackCost; // Deduct stamina
                if (player.Stamina < 0) player.Stamina = 0;
                player.UpdateStaminaBar();
            }
        }
    }

    private void Rotation()
    {
        targetRotation = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(targetRotation.y, targetRotation.x) * Mathf.Rad2Deg;
        if (canRotate)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (angle > 90 || angle < -90)
            {
                pistolSR.flipY = true;
            }
            else
            {
                pistolSR.flipY = false;
            }
        }
    }

    private Bullet CreatePoolItem()
    {
        Bullet newBullet = Instantiate(bullet, transform.position, new Quaternion(0, 0, 0, 0), parentTransform);
        newBullet.gameObject.SetActive(false);
        newBullet.Pool = bulletPool;
        return newBullet;
    }
    private void OnTakeFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.gameObject.transform.position = transform.position;
    }
    private void OnReturnedFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void OnDestroyObject(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
