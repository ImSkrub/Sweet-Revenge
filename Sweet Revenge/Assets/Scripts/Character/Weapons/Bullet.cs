using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int speed = 50;
    private Vector3 direction = Vector3.right;
    private ObjectPool<Bullet> bulletPool;
    private bool isReleased = false;
    private int baseDamage = 25;
    public int damage;
    public int Damage
    {
        get => damage;
        set => damage = Mathf.Max(0, value); // Setter with validation
    }
    public ObjectPool<Bullet> Pool
    {
        get { return bulletPool; }
        set { bulletPool = value; }
    }

    private void Start()
    {
        damage = baseDamage;
    }

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

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized; // Ensure the direction is normalized
    }

    private void ApplyDamage(int damage, Collider2D enemy)
    {
        IDamageable damageable = enemy.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("holaaaaaaa");
            ApplyDamage(damage, collision.collider);
            Deactivate();
        }
    }
}
