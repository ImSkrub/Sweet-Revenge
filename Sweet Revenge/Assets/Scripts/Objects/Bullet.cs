using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private int speed = 100;
    private Vector3 direction = Vector3.right;
    private ObjectPool<Bullet> bulletPool;
    private bool isReleased = false;
    private int damage = 25;
    public ObjectPool<Bullet> Pool { get { return bulletPool; } set { bulletPool = value; } }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);

    }
    private void OnEnable()
    {
        isReleased = false;
        Invoke("Deactivate", 1f);
    }
    private void OnDisable()
    {
        CancelInvoke("Deactivate");
    }
    private void Deactivate()
    {
        if (bulletPool != null && !isReleased)
        {
            bulletPool.Release(this);
            isReleased = true;
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    private void ApplyDamage(int damage, Collider2D enemy)
    {
        IDamageable damageable = enemy.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ApplyDamage(damage, collision);
            Deactivate();
        }
    }
}
