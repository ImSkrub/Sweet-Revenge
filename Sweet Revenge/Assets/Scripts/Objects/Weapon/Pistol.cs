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
    [SerializeField] private Bullet bulletPrefab;
    private ObjectPool<Bullet> bulletPool;
    private Transform parentTransform;

    private Vector3 targetRotation;
    private SpriteRenderer pistolSR;
    [SerializeField] private Player player;
    //booleans
    public bool canRotate = false;


    private void Awake()
    {
        bulletPool = new ObjectPool<Bullet>(CreatePoolItem, OnTakeFromPool, OnReturnedFromPool, OnDestroyObject, true, 10, valueGun.maxBullets);
    }
    private void Start()
    {
        pistolSR = GetComponent<SpriteRenderer>();
        parentTransform = GetComponentInParent<Transform>();
        player = FindObjectOfType<Player>();
        player.equipped += Gun_equipped;
    }

    private void Gun_equipped()
    {
        canRotate = true;
    }

    private void Update()
    {
        if (canRotate)
        {
            Rotation();
        }
    }

    public void Attack()
    {
        bulletPool.Get();
    }
    private void Rotation()
    {
        targetRotation = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(targetRotation.y, targetRotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        pistolSR.flipY = angle > 90 || angle < -90;
    }

    private Bullet CreatePoolItem()
    {
        Bullet newBullet = Instantiate(bulletPrefab, transform.position, new Quaternion(0, 0, 0, 0), parentTransform);
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

