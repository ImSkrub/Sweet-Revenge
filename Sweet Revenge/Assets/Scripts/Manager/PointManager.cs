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
    private int shopCoin = 0;
    private int doorCoin = 0;
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

    private void Update()
    {
        doorCoinText.text = $"{doorCoin}";
        shopCoinText.text = $"{shopCoin}";
    }

    public void AddShopCoin(int value)
    {
        shopCoin += value;
    }
    public void AddDoorCoin(int value)
    {
       doorCoin += value;
    }
    public void RestartDoorCoin()
    {
        doorCoin = 0;
    }
   
}
