using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int _shopCoin
    {
        get => shopCoin;
        set => shopCoin = Mathf.Max(0, value);
    }

    public int _doorCoin
    {
        get => doorCoin;
        set => doorCoin = Mathf.Max(0, value);
    }

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
        SceneManager.sceneLoaded += OnSceneLoaded; // Suscribirse al evento de escena cargada
        UpdateCoinTextReferences();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Desuscribirse al evento
    }

    private void Update()
    {
        
        if (doorCoinText != null && shopCoinText != null)
        {
            UpdateCoinTexts();
        }
    }

    private void UpdateCoinTextReferences()
    {
        // Buscar los textos en la escena actual
        GameObject doorCoinObject = GameObject.FindGameObjectWithTag("DoorCoin");
        GameObject shopCoinObject = GameObject.FindGameObjectWithTag("ShopCoin");

        doorCoinText = doorCoinObject?.GetComponent<TMP_Text>();
        shopCoinText = shopCoinObject?.GetComponent<TMP_Text>();
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
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Buscar y actualizar los textos nuevamente cuando se cargue una nueva escena
        UpdateCoinTextReferences();
        UpdateCoinTexts();
    }
}