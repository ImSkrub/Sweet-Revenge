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
    public TextMeshProUGUI purchaseText;
    public bool activeUI = false;
    [SerializeField] private int cost = 500;
    private int currentCoins;
    private int maxPurchases = 3;
    private int currentPurchases = 0;
    private bool hasPowerUp = false;

    [SerializeField] Transform spawnpoint;
    [SerializeField] private AudioClip clip, reviveClip;

    private bool isRestoring = false;

    private void Start()
    {
        if (PointManager.Instance == null)
        {
            Debug.LogError("PointManager instance is null. Make sure it is initialized before GameStatusManager.");
            return;
        }

        player.OnDeath += Checkpoint;
        currentCoins = PointManager.Instance._doorCoin;
        UpdatePurchaseText();
    }

    private void LateUpdate()
    {
        currentCoins = PointManager.Instance._doorCoin;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && activeUI && currentPurchases < maxPurchases && !hasPowerUp)
        {
            if (currentCoins >= cost)
            {
                SoundFXManager.instance.PlaySoundFXClip(clip, transform, 1f);
                savedStates.Push(player.SaveState(spawnpoint.position));
                currentPurchases++;
                hasPowerUp = true;
                currentCoins -= cost;
                PointManager.Instance.AddDoorCoin(-cost);
                UpdatePurchaseText(); // Update the text after the purchase
                ToggleUI();
                Debug.Log("Saved State");
            }
            else
            {
                Debug.Log("Not enough coins to purchase.");
            }
        }
      
    }

    public void Checkpoint()
    {
        if (isRestoring) return;
        isRestoring = true;

        if (HasSavedStates())
        {
            PlayerMemento lastSavedState = savedStates.Pop();
            player.RestoreState(lastSavedState);
            Debug.Log("Estado restaurado");
            SoundFXManager.instance.PlaySoundFXClip(reviveClip, transform, 1f);
        }
        else
        {
            Debug.Log("No saved states available to restore.");
        }
        isRestoring= false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activeUI = true; // Set activeUI to true
            ToggleUI();
            UpdatePurchaseText(); // Update the text to show purchase options
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activeUI = false; // Set activeUI to false
            ToggleUI();
            purchaseText.text = ""; // Clear the text when the player exits
        }
    }

    public void ToggleUI()
    {
        _UI.SetActive(!_UI.activeSelf);
    }

    private void UpdatePurchaseText()
    {
        // Only read currentCoins, do not modify it here
        if (currentPurchases < maxPurchases && currentCoins >= cost)
        {
            purchaseText.text = $"Press 'E' to buy power up ${cost}\n{maxPurchases - currentPurchases} purchases left.";
        }
        else if (currentPurchases >= maxPurchases)
        {
            purchaseText.text = "You have reached the maximum purchases.";
        }
        else
        {
            purchaseText.text = $"You are missing {cost - currentCoins} coins.";
        }
    }

    public void UsePowerUp()
    {
        hasPowerUp = false; // Reset the power-up status
        UpdatePurchaseText();
    }

    public bool HasSavedStates()
    {
        return savedStates.Count > 0;
        
    }

    public void ResetStates()
    {
        savedStates.Clear();
        Debug.Log("Saved states have been Reset.");
    }

}
