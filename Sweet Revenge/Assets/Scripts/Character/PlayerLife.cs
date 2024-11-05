using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    //Parametres
    [Header("Parametres")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float currentHealth;
    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = Mathf.Max(0, value); // Setter with validation
    }

    private SpriteRenderer spriteRenderer;
   // private Animation anim;
    private float currentTime;
    public event Action OnDeath;
    
    //Color on hit
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private Color lifeGainColor = Color.green;
    private Color originalColor;

    [SerializeField] private Image lifeBar;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    private void Start()
    {
        //anim = GetComponent<Animation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    private void Update()
    {
        lifeBar.fillAmount =currentHealth/maxHealth;
        currentTime += Time.deltaTime;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("murio");
            Death();
        }
    }

    public void GetDamage(float value)
    {
        currentHealth -= value;
        spriteRenderer.color = damageColor;
        Invoke("RestoreColor", 0.5F);
    }
    public void RestoreLife(int value)
    {
        currentHealth += value;
        spriteRenderer.color = lifeGainColor;
        Invoke("RestoreColor", 0.5f);
    }

    private void RestoreColor()
    {
        spriteRenderer.color = originalColor;
    }

    public void Death()
    {
        //anim.SetTrigger("Death");
        OnDeath?.Invoke();
        this.gameObject.SetActive(false);
    }

    public PlayerMemento SaveState()
    {
        return new PlayerMemento(transform.position, maxHealth);
    }

    public void RestoreState(PlayerMemento state)
    {
        transform.position = state.position;
        this.currentHealth = state.health;
    }
}
