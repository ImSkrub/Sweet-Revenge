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

        // Busca los objetos en la escena por su nombre
        GameObject doorCoinObject = GameObject.FindGameObjectWithTag("DoorCoin");
        GameObject shopCoinObject = GameObject.FindGameObjectWithTag("ShopCoin");

        // Asegúrate de que los objetos se encontraron antes de intentar acceder a sus componentes
        if (doorCoinObject != null)
        {
            doorCoinText = doorCoinObject.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto 'DoorCoin' en la escena.");
        }

        if (shopCoinObject != null)
        {
            shopCoinText = shopCoinObject.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto 'ShopCoin' en la escena.");
        }
    }

    private void Update()
    {
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
