using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour,IDamageable
{
    [Header("Enemy Life")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float destroyDelay = 0.5f;
    public float enemyHealth { get { return enemyHealth; } set { if (enemyHealth >= 0) health = value; } }
    private bool isDead = false;
    
    [Space(3)]
    [Header("Color")]
    private SpriteRenderer sr;
    [SerializeField] private Color damageColor = Color.red;
    private Color originalColor;
    //[Header("Animator")]
    //private Animator anim;
    private void Awake()
    {
        health = maxHealth;
        //anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    private void Update()
    {
        if(health<=0f && !isDead)
        {
            Death();
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        sr.color = damageColor;
        Invoke("RestoreColor", damageCooldown);
    }

    private void RestoreColor()
    {
       sr.color = originalColor;
    }

    private void Death()
    {
        //anim.SetTrigger("die");
        isDead = true;
        Destroy(gameObject,destroyDelay);
    }

}
