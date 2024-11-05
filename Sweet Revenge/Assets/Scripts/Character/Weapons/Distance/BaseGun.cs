using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public abstract class BaseGun : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] protected Parameters valueGun; // Use protected to allow derived classes to access it
    [SerializeField] protected Bullet bulletPrefab;
    protected ObjectPool<Bullet> bulletPool;
    protected Transform parentTransform;

    protected virtual void Awake()
    {
        bulletPool = new ObjectPool<Bullet>(CreatePoolItem, OnTakeFromPool, OnReturnedFromPool, OnDestroyObject,true,25,30);
    }
    protected virtual void Start()
    {
       parentTransform = GetComponentInParent<Transform>();
        // Ensure bulletPool is assigned
        if (bulletPool == null)
        {
            Debug.LogError("Bullet Pool is not assigned in the Shotgun script.");
        }
        if (parentTransform == null)
        {
            Debug.LogError("Parent Transform is not assigned in the Shotgun script.");
        }
        // Ensure valueGun is assigned
        if (valueGun == null)
        {
            Debug.LogError("ValueGun is not assigned in the Shotgun script.");
        }
    }

    /*
    //private void Gun_equipped()
    //{
    //    canRotate = true;
    //}

    //private void Update()
    //{
    //    if (canRotate)
    //    {
    //        Rotation();
    //    }
    //}*/

    /*
    private void Rotation()
    {
        targetRotation = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(targetRotation.y, targetRotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        pistolSR.flipY = angle > 90 || angle < -90;
    }
    */
    protected virtual Bullet CreatePoolItem()
    {
        Bullet newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, parentTransform);
        newBullet.gameObject.SetActive(false);
        newBullet.Pool = bulletPool;

        // Ensure bulletPrefab is not null and has a valid parameters property
        if (bulletPrefab != null && valueGun != null)
        {
            newBullet.parameters = valueGun; // Assuming parameters is a field in Bullet
        }
        else
        {
            Debug.LogError("BulletPrefab or ValueGun is null when creating bullet pool item.");
        }

        return newBullet;
    }
    protected virtual void OnTakeFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.gameObject.transform.position = transform.position;
    }
    protected virtual void OnReturnedFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    protected virtual void OnDestroyObject(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
    public abstract void Attack();
}

