using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameStatusManager : MonoBehaviour
{
    private Stack<PlayerMemento> savedStates = new Stack<PlayerMemento>();
    public PlayerLife player;
    public GameObject _UI;
    public TextMeshProUGUI purchaseText; // Reference to the TextMeshPro component
    public bool activeUI = false;
    private int maxPurchases = 3;
    private int currentPurchases = 0;
    private bool hasPowerUp = false; // New variable to track if the player has a power-up

    private void Awake()
    {
        player.OnDeath += Checkpoint;
        UpdatePurchaseText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && activeUI && currentPurchases < maxPurchases && !hasPowerUp)
        {
            savedStates.Push(player.SaveState());
            currentPurchases++;
            hasPowerUp = true; // Set to true when a power-up is purchased
            UpdatePurchaseText();
            ToggleUI(); // Deactivate the UI when the purchase is made
            Debug.Log("Saved State");
        }
    }

    public void Checkpoint()
    {
        if (savedStates.Count > 0)
        {
            PlayerMemento lastSavedState = savedStates.Pop();
            player.RestoreState(lastSavedState);
            Debug.Log("Estado restaurado");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activeUI = true; // Set activeUI to true
            ToggleUI(); // Show the UI
            UpdatePurchaseText(); // Update the text to show purchase options
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activeUI = false; // Set activeUI to false
            ToggleUI(); // Hide the UI
            purchaseText.text = ""; // Clear the text when the player exits
        }
    }

    public void ToggleUI()
    {
        _UI.SetActive(!_UI.activeSelf);
    }

    private void UpdatePurchaseText()
    {
        if (currentPurchases < maxPurchases)
        {
            purchaseText.text = $"Press 'E' to buy power up. {maxPurchases - currentPurchases} out of {maxPurchases} left.";
        }
        else
        {
            purchaseText.text = "You have reached the maximum purchases.";
            // Optionally deactivate the UI or the text
            // _UI.SetActive(false);
        }
    }

    // Call this method when the power-up effect is used or expires
    public void UsePowerUp()
    {
        hasPowerUp = false; // Reset the power-up status
        UpdatePurchaseText(); // Update the UI text to reflect the change
    }
    public bool HasSavedStates()
    {
        return savedStates.Count > 0;
    }
}
