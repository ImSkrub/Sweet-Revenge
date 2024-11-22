using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Animator animator;
    public Rigidbody2D rb;
    private Transform jugador;
    private bool lookingRight = true;

    [Header("References")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private BarraDeVida barraDeVida;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        enemyHealth = GetComponent<EnemyHealth>();
        barraDeVida.InicializarBarraVida(enemyHealth.enemyHealth);
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void TakeDamage(float damage)
    {
        enemyHealth.enemyHealth -= damage;
        barraDeVida.CambiarVidaActual(enemyHealth.enemyHealth);
        if (enemyHealth.enemyHealth <= 0)
        {
            animator.SetTrigger("Muerte");
        }
    }
    public void LookAtPlayer()
    {
        if ((jugador.position.x > transform.position.x && !lookingRight) || (jugador.position.x < transform.position.x && lookingRight))
        {
            lookingRight = !lookingRight;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }
}
