using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    //Singleton
    private static PointManager instance;
    public static PointManager Instance => instance;
    [SerializeField] private TMP_Text doorCoinText;
    [SerializeField] private TMP_Text shopCoinText;
    
    [Header("Coins")]
    [SerializeField] private int shopCoin = 0;
    [SerializeField] private int doorCoin = 0;
    public int _shopCoin => shopCoin;
    public int _doorCoin => doorCoin;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
    }

    private void Start()
    {
        // Initialize the text display
        UpdateCoinTexts();
    }

    // Method to update the coin text displays
    private void UpdateCoinTexts()
    {
        doorCoinText.SetText($"{doorCoin}");
        shopCoinText.SetText($"{shopCoin}");
    }

    // Method to add shop coins
    public void AddShopCoin(int value)
    {
        shopCoin += value;
        UpdateCoinTexts(); // Update the text display
    }

    // Method to add door coins
    public void AddDoorCoin(int value)
    {
        doorCoin += value;
        UpdateCoinTexts(); // Update the text display
    }

    // Method to reset door coins
    public void RestartDoorCoin()
    {
        doorCoin = 0;
        UpdateCoinTexts(); // Update the text display
    }

}
