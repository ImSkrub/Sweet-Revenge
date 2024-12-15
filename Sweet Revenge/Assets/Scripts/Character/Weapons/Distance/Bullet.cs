using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private int speed = 100;
    private Vector3 direction = Vector3.zero;
    private ObjectPool<Bullet> bulletPool;
    private bool isReleased = false;

    // Reference to the Parameters ScriptableObject
    [SerializeField] public Parameters parameters; 

    private int damage;
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
        // Set the damage from the Parameters object
        if (parameters != null)
        {
            damage = (int)parameters.damage; // Use the damage from Parameters
        }
        else
        {
            Debug.LogWarning("Parameters not set for Bullet. Defaulting damage to 0.");
            damage = 0; // Default value if parameters are not set
        }
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        
    }
    private void OnEnable()
    {
        isReleased = false;
        Invoke("Deactivate", 10f);
    }
    private void OnDisable()
    {
        CancelInvoke("Deactivate");
    }
    private void Deactivate()
    {
        if (bulletPool != null && !isReleased)
        {
            direction = Vector3.zero;
            bulletPool.Release(this);
            isReleased = true;
        }
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized; // Ensure the direction is normalized
    }

    private void ApplyDamage(int damage, Collider2D enemy)
    {
        // Log the enemy's name to see which object is being hit
        Debug.Log($"Attempting to apply {damage} damage to {enemy.gameObject.name}");

        IDamageable damageable = enemy.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Log that the damageable component was found
            Debug.Log($"Damageable component found on {enemy.gameObject.name}. Applying damage.");
            damageable.TakeDamage(damage);
        }
        else
        {
            // Log that the enemy does not implement IDamageable
            Debug.LogWarning($"No IDamageable component found on {enemy.gameObject.name}. Damage not applied.");
        }
    }
    public void ApplyKnockback(float knockback)
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ApplyDamage(damage, collision);
            Deactivate();
        }
        if (collision.gameObject.CompareTag("Door") || collision.gameObject.CompareTag("Wall"))
        {
            Deactivate();
        }
    }
}
