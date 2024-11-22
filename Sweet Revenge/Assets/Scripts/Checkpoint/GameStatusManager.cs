using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    [SerializeField] private int cost=500;
    private int currentCoins;
    private int maxPurchases = 3;
    private int currentPurchases = 0;
    private bool hasPowerUp = false; // New variable to track if the player has a power-up

    [SerializeField] Transform spawnpoint;

    private void Start()
    {
        if (PointManager.Instance == null)
        {
            Debug.LogError("PointManager instance is null. Make sure it is initialized before GameStatusManager.");
            return; // Exit the method to prevent further null reference issues
        }

        player.OnDeath += Checkpoint;
        UpdatePurchaseText();
        currentCoins = PointManager.Instance._doorCoin;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && activeUI && currentPurchases < maxPurchases &&
            !hasPowerUp)
        {
            savedStates.Push(player.SaveState(spawnpoint.position));
            currentPurchases++;
            hasPowerUp = true; // Set to true when a power-up is purchased
            UpdatePurchaseText();
            ToggleUI(); // Deactivate the UI when the purchase is made
            Debug.Log("Saved State");
        }
    }

    public void Checkpoint()
    {
        if (HasSavedStates())
        {
            PlayerMemento lastSavedState = savedStates.Pop();
            player.RestoreState(lastSavedState);
            Debug.Log("Estado restaurado");
        }
        else
        {
            Debug.Log("No saved states available to restore.");
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
        if (currentPurchases < maxPurchases && cost <= currentCoins)
        {
            purchaseText.text = $"Press 'E' to buy power up ${cost}\n {maxPurchases - currentPurchases} out of {maxPurchases} left.";
            PointManager.Instance.AddDoorCoin(-cost);
        }
        else if(currentPurchases > maxPurchases)
        {
            purchaseText.text = "You have reached the maximum purchases.";
        }
        else if(cost >= currentCoins)
        {
            purchaseText.text = $"You are missing {currentCoins - cost} coins";
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
