using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour,IDamageable
{
    private Animator animator;
    public Rigidbody2D rb;
    public Transform playerTransform;
    private bool lookingRight = true;

    [Header("Parameters")]
    [SerializeField] private float life;
    [SerializeField] private float maxLife = 2500;
    [SerializeField] private float damageCooldown = 0.5f;
    [SerializeField] private float destroyDelay = 0.5f;
    private float lastAttackTime = 0f;

    [Header("References")]
    [SerializeField] private LayerMask playerLayer;
    private PlayerLife playerHealth;
    private Rigidbody2D playerRb; // Almacenamos el Rigidbody del jugador
    private bool isDead = false;

    [Header("Audios")]
    [SerializeField] private AudioClip[] damageSoundClips;

    [Space(3)]
    [Header("Color")]
    [SerializeField] public SpriteRenderer sr;
    [SerializeField] private Color damageColor = Color.red;
    private Color originalColor;

    [SerializeField] private Slider healthBar;

    public event Action OnDeath;

    private IBossState bossState;

    [SerializeField] private float knockbackRange = 5f;

    private void Awake()
    {
        life = maxLife;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerHealth = FindObjectOfType<PlayerLife>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        // Obtener el Rigidbody2D del jugador solo una vez
        playerRb = playerTransform.GetComponent<Rigidbody2D>();

        SetState(new Phase1State());
    }

    private void Update()
    {
        healthBar.value = life / maxLife;
        if (isDead) return;

        bossState.UpdateState();

        if (life <= 1500 && !(bossState is Phase2State))
        {
            TransitionToPhaseTwo();
        }
        if (life <= 0 && (bossState is Phase2State))
        {
            Invoke("Death", 2f);
        }
        Debug.Log(isKnockbackActive);
        if (isKnockbackActive && !IsPlayerInKnockbackRange())
        {
            isKnockbackActive = false;  // Se desactiva el knockback cuando el jugador sale del rango
        }
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        sr.color = damageColor;
        Invoke("RestoreColor", damageCooldown);
        if (life <= 1500)
        {
            animator.SetTrigger("Transformation");
        }
    }

    public void FollowPlayer(float moveSpeed)
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
        LookAtPlayer();
    }

    public void LookAtPlayer()
    {
        Vector2 direction = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        animator.SetFloat("MovY", direction.x);
        angle += 180f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void RestoreColor()
    {
        sr.color = originalColor;
    }

    public bool PlayerInSight()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, bossState.GetRange(), playerLayer);
        Debug.DrawRay(transform.position, directionToPlayer * bossState.GetRange(), Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            playerHealth = hit.transform.GetComponent<PlayerLife>();
            return true;
        }
        return false;
    }

    private bool isKnockbackActive = false; // Nueva variable para controlar el knockback

    public void Attack(float damage)
    {
        if (PlayerInSight() && Time.time - lastAttackTime >= damageCooldown && !isKnockbackActive) // Añadimos control sobre knockback
        {
            lastAttackTime = Time.time;  // Actualiza el tiempo de último ataque

            playerHealth.GetDamage(damage);

            //// Aplica Knockback solo una vez
            Vector2 knockbackDirection = (playerTransform.position - transform.position).normalized;
            playerRb.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse);

            //// Activar el estado de knockback
            isKnockbackActive = true;
            
        }
    }
    private bool IsPlayerInKnockbackRange()
    {
        return Vector2.Distance(transform.position, playerTransform.position) <= knockbackRange;
    }

   

    private void TransitionToPhaseTwo()
    {
        SetState(new Phase2State());
        animator.SetTrigger("Transformation");
    }

    private void SetState(IBossState newState)
    {
        bossState?.Exit();
        bossState = newState;
        bossState.Enter(this);
    }

    private void Death()
    {
        isDead = true;
        OnDeath?.Invoke();
        Destroy(gameObject, destroyDelay);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.GetDamage(5f);
        }
    }

    private void OnDrawGizmos()
    {
        if (bossState == null) return;
        Debug.Log("Drawing Gizmo with range: " + bossState.GetRange());
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bossState.GetRange());
    }
}
