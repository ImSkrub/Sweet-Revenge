using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    //Parametres
    [Header("Parametres")]
    [SerializeField] private float currentHealth;
    public float CurrentHealth => currentHealth;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private SpriteRenderer spriteRenderer;
   // [SerializeField] private Animation anim;
    private float currentTime;
    public event Action onDeath;
    
    //Color on hit
    public Color damageColor= Color.red;
    private Color originalColor;

    [SerializeField] private Image lifeBar;

    private void Start()
    {
        //anim = GetComponent<Animation>();
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    private void Update()
    {
        lifeBar.fillAmount =currentHealth/maxHealth;
        currentTime = Time.deltaTime;
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void GetDamage(float value)
    {
        currentHealth -= value;
        spriteRenderer.color = damageColor;
        Invoke("RestoreColor", 0.5F);
    }

    private void RestoreColor()
    {
        spriteRenderer.color = originalColor;
    }


    public void Death()
    {
        //anim.SetTrigger("Death");
        onDeath?.Invoke();
        this.gameObject.SetActive(false);
    }

}
