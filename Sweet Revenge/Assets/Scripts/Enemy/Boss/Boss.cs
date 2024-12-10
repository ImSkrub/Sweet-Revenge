using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IDamageable
{
    private Animator animator;
    public Rigidbody2D rb;
    public Transform playerTransform;


    [Header("Parameters")]
    [SerializeField] private float life;
    [SerializeField] private float maxLife = 300;
    [SerializeField] private float damageCooldown = 0.5f;
    [SerializeField] private float destroyDelay = 0.5f;
    private float transitionLife = 500;
    //Knockback
    [SerializeField] private float KBCounter;
    [SerializeField] private float KBTotalTime;
    [SerializeField] private float KBDelay = 0.5f;
    [SerializeField] private float KnockbackForce = 5f;
    public float scaleVelocity = 1f;
    private float scaleVelocityMax = 2;


    [Header("References")]
    [SerializeField] private LayerMask playerLayer;
    private PlayerLife playerHealth;
    private Rigidbody2D playerRb; // Almacenamos el Rigidbody del jugador
    [SerializeField] Rigidbody2D bossRb;
    public bool isDead = false, isFollowing = false;
    [Space(3)]
    [Header("Audios")]
    [SerializeField] private AudioClip[] damageSoundClips;
    [SerializeField] private AudioClip attackSound, deathSound;

    [Space(3)]
    [Header("Color")]
    [SerializeField] public SpriteRenderer sr;
    [SerializeField] private Color damageColor = Color.red;
    private Color originalColor;
    public Color OriginalColor
    {
        get { return originalColor; }
        set { originalColor = value; }
    }
    [Header("UI")]
    [SerializeField] private GameObject healthBarCanvas;
    [SerializeField] private Image healthBar;

    public bool cinematicFinished = false;

    public event Action OnDeath;


    private IBossState bossState;


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

    private void Start()
    {
        healthBarCanvas.SetActive(false);
    }

    private void Update()
    {
        healthBar.fillAmount= life / maxLife;
        if (isDead) return;
        bossState.UpdateState();
       
        if (life <= transitionLife && !(bossState is Phase2State))
        {
            TransitionToPhaseTwo();
        }
        if (life <= 0)
        {
            Death();
        }

        //scaleVelocity += scaleVelocity * Time.deltaTime;
        //if (scaleVelocity > scaleVelocityMax) scaleVelocity = scaleVelocityMax;
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            life -= damage;
            sr.color = damageColor;
            Invoke("RestoreColor", damageCooldown);
            SoundFXManager.instance.PlayRandomSoundFXClip(damageSoundClips, transform, 1f);

            // Verifica si la vida es menor o igual a cero
            if (life <= 0)
            {
                life = 0; 
                Death();
            }
            else if (life <= transitionLife && !(bossState is Phase2State))
            {
                //animator.SetTrigger("Transformation");
                TransitionToPhaseTwo();
            }
        }
    }

    public void FollowPlayer(float moveSpeed)
    {
        if (!isDead&& cinematicFinished)
        {
            isFollowing = true;
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
            LookAtPlayer();
            animator.SetBool("Walk", true);
        }
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



    public void Attack(float damage)
    {
        if (!isDead)
        {
            if (PlayerInSight()) // Añadimos control sobre knockback
            {
                playerHealth.GetDamage(damage);

                Vector2 knockbackDirection = (playerTransform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * KnockbackForce, ForceMode2D.Impulse);
                StartCoroutine(ResetKB());
                playerRb.velocity = Vector2.zero;
                animator.SetTrigger("Attack");
                SoundFXManager.instance.PlaySoundFXClip(attackSound, transform, 1f);
                Debug.Log(damage);
            }
        }
    }

    private IEnumerator ResetKB()
    {
        yield return new WaitForSeconds(KBDelay);
        
    }


    private void TransitionToPhaseTwo()
    {
        if (bossState is Phase1State)
        {
            SetState(new Phase2State());
            //animator.SetTrigger("Transformation");
            sr.color = Color.magenta;
        }
       
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
        animator.SetBool("isDead", isDead);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Invoke("HandleBossDeath", 1.5f);
        SoundFXManager.instance.PlaySoundFXClip(deathSound, transform, 1f);
        //Destroy(gameObject, destroyDelay);
    }

    private void HandleBossDeath()
    {
        GameManager.Instance.WinGame();
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.collider.tag == "Player" && !isDead)
    //    {
    //        playerHealth.GetDamage(1f * Time.deltaTime);

    //    }
    //}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            playerHealth.GetDamage(10f * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
