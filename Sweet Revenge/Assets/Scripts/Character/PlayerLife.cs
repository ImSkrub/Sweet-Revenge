using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    // Parameters
    [Header("Parameters")]
    [SerializeField] public float maxHealth = 100;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float currentHealth;
    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = Mathf.Max(0, value); // Setter with validation
    }

    private SpriteRenderer spriteRenderer;
    private float currentTime;
    public event Action OnDeath;

    // Color on hit
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private Color lifeGainColor = Color.green;
    private Color originalColor;

    [SerializeField] private Image lifeBar;

    // Reference to PlayerCheckpoint
    private GameStatusManager playerCheckpoint;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // Find the PlayerCheckpoint in the scene
        playerCheckpoint = FindObjectOfType<GameStatusManager>();
        if (playerCheckpoint == null)
        {
            Debug.LogError("PlayerCheckpoint not found in the scene.");
        }
    }

    private void Update()
    {
        lifeBar.fillAmount = currentHealth / maxHealth;
        currentTime += Time.deltaTime;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player died");
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
        // Check if there are any saved states before deactivating
        if (playerCheckpoint != null && playerCheckpoint.HasSavedStates())
        {
            // If there are saved states, restore the player's state
            playerCheckpoint.Checkpoint(); // Restore the last saved state
            Debug.Log("Player restored from checkpoint.");
        }
        else
        {
            // If there are no saved states, deactivate the player
            Debug.Log("No saved states available. Player is dead.");
            this.gameObject.SetActive(false);
        }
    }

    public PlayerMemento SaveState(Vector3 position)
    {
        return new PlayerMemento(position, maxHealth);
    }

    public void RestoreState(PlayerMemento state)
    {
        gameObject.SetActive(true);
        transform.position = state.position;
        this.currentHealth = state.health;
    }
}
