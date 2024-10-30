using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private EnemiesValues enemyData;
    private float currentTime;


    [Header("References")]
    public string Name;
    private Rigidbody2D rb;
    [SerializeField]private Transform target;
    private PlayerLife playerHealth;
    private EnemyHealth enemyHealth;
    private Vector3 enemyDirection;
    private bool isFacingRight = true;
    public event Action OnDeath;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        target = FindObjectOfType<Player>().transform;
    }

    private void FixedUpdate()
    {
        rb.velocity = enemyDirection.normalized * enemyData.velocity;
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        //anim
    }
    public void Death()
    {
        Destroy(gameObject,0.5f);
        OnDeath?.Invoke();
    }
    //colision con jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentTime >= enemyData.attackCooldown)
        {
            if (collision.gameObject.layer == 3)
            {
                collision.gameObject.GetComponent<PlayerLife>().GetDamage(enemyData.damage);
                currentTime = 0;
            }
        }
    }
}
