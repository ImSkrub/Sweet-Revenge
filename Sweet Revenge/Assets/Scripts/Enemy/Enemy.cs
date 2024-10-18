using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int velocity = 10;
    [SerializeField] private float rangeToAttack;
    [SerializeField] private float closestDist;
    [SerializeField] private float lerpSpeedRotation;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    private float currentTime;


    [Header("References")]
    public string Name;
    private Rigidbody2D rb;
    [SerializeField]private Transform target;
    private PlayerLife playerHealth;
    private Vector3 enemyDirection;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim
    }

    private void FixedUpdate()
    {
        rb.velocity = enemyDirection.normalized * velocity;
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        //anim
    }
    public void OnDeath()
    {
        Destroy(gameObject,0.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangeToAttack);
    }

    //colision con jugador
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (currentTime >= attackCooldown)
        {
            if (collision.gameObject.layer == 3)
            {
                collision.gameObject.GetComponent<PlayerLife>().GetDamage(damage);
                currentTime = 0;
            }
        }
    }
}
