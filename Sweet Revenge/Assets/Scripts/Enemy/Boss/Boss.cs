using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private Animator animator;
    public Rigidbody2D rb;
    public Transform jugador;
    private bool lookingRight = true;
    
    
    [Header("Parameters")]
    [SerializeField] private float life;
    [SerializeField] private float maxLife = 2500;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float destroyDelay = 0.5f;
    [Header("References")]
    [SerializeField] private LayerMask playerLayer;
    private PlayerLife playerHealth;
    private bool isDead = false;

    [Header("Audios")]
    [SerializeField] private AudioClip[] damageSoundClips;

    [Space(3)]
    [Header("Color")]
    private SpriteRenderer sr;
    [SerializeField] private Color damageColor = Color.red;
    private Color originalColor;

    [SerializeField] Slider healthBar;

    public event Action OnDeath;

    private IBossState bossState;
   
    private void Awake()
    {
        life=maxLife;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();

        originalColor = sr.color;
        playerHealth = FindObjectOfType<PlayerLife>();

        SetState(new Phase1State());
    }
    private void Update()
    {
        healthBar.value = life / maxLife;
        if (isDead) return;
       bossState.UpdateState();
       if(life<= 1500 && !(bossState is Phase2State))
       {
            TransitionToPhaseTwo();
       }
       if (life <= 0 && (bossState is Phase2State))
       {
            //anim death
            Invoke("Death", 2f);
          
       }
    }

    public void TakeDamage(float damage)
    {
        life -=damage;
        sr.color = damageColor;
        Invoke("RestoreColor", damageCooldown);
        // barraDeVida.CambiarVidaActual(life);

        if (life <= 1500)
        {
            animator.SetTrigger("Transformation");
            //pasar de fase --> cambiar el modo en el que ataca al player.
        }
    }

    public void FollowPlayer(float moveSpeed)
    {
        Vector2 direction = (jugador.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
        LookAtPlayer();
    }

    public void LookAtPlayer()
    {
        Vector2 direction = jugador.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        animator.SetFloat("MovY",direction.x);
        Debug.DrawLine(transform.position, jugador.position, Color.red); // Draw a line to the player
        Debug.Log("Angle: " + angle); // Log the angle for debugging
        angle += 180f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void RestoreColor()
    {
        sr.color = originalColor;
    }

    public bool PlayerInSight()
    {
        //direction to player
        Vector2 directionToPlayer = (jugador.position - transform.position).normalized;
        //Cast a ray from enemy to player. Make sure player has PlayerLife script
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, bossState.GetRange(), playerLayer);

        //drawing the ray in scene.
        Debug.DrawRay(transform.position, directionToPlayer * bossState.GetRange(), Color.red);


        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            playerHealth = hit.transform.GetComponent<PlayerLife>();
            return true;
        }
        return false;
    }

    public void Attack(float damage)
    {
        if (PlayerInSight())
        {
            playerHealth.GetDamage(damage);
            Vector2 knockbackDirection = (jugador.position - transform.position).normalized;
            jugador.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * bossState.GetKnockbackForce(), ForceMode2D.Impulse);
            
        }
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
        //animator.SetTrigger("death");
        isDead =true;
        OnDeath?.Invoke();
        Destroy(gameObject,0.5f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.GetDamage(5f);
        }
    }
}
