using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    [Header("Items for shop")]
    [SerializeField] private Item[] items;
    [Space(3)]
    [Header("References")]
    [SerializeField] private GameObject shopUI;
    [SerializeField] private TMP_Text coinText;
    public Transform shopContent;
    public GameObject itemPrefab;
    private Player player;
    private int currentCoins = 0;

    // Reference to the spawn point
    [SerializeField] private Transform itemSpawnPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        currentCoins = PointManager.Instance._shopCoin;
        foreach (Item item in items)
        {
            GameObject _item = Instantiate(itemPrefab, shopContent);
            item.itemRef = _item;
            foreach (Transform child in _item.transform)
            {
                if (child.gameObject.name == "Quantity")
                {
                    child.gameObject.GetComponent<TMP_Text>().SetText(item.quantity.ToString());
                }
                else if (child.gameObject.name == "Cost")
                {
                    child.gameObject.GetComponent<TMP_Text>().SetText(item.cost.ToString());
                }
                else if (child.gameObject.name == "Name")
                {
                    child.gameObject.GetComponent<TMP_Text>().SetText(item.itemName.ToString());
                }
                else if (child.gameObject.name == "Image")
                {
                    child.gameObject.GetComponent<Image>().sprite = item.image;
                }
            }
            _item.GetComponent<Button>().onClick.AddListener(() => { BuyItem(item); });
        }
    }

    public void BuyItem(Item item)
    {
        if (currentCoins >= item.cost)
        {
            currentCoins -= item.cost;
            item.quantity++;
            item.itemRef.transform.GetChild(0).GetComponent<TMP_Text>().SetText(item.quantity.ToString());
            //ApplyItem(item);
            SpawnItem(item); // Spawn the item in the world
        }
    }

    private void SpawnItem(Item item)
    {
        GameObject spawnedItem = Instantiate(item.itemPrefab, itemSpawnPoint.position, Quaternion.identity);
    }

    //Usar ApplyItem si queremos hacer items de modificacion no temporal.
    public void ApplyItem(Item item)
    {
        switch (item.itemName)
        {
            case "Warhammer":
                player.SetWeapon(item.itemRef.gameObject.GetComponent<IWeapon>());
                break;
            case "SpikeBat":
                player.SetWeapon(item.itemRef.gameObject.GetComponent<IWeapon>());
                break;
            
        }
    }

    public void ToggleShop()
    {
        shopUI.SetActive(!shopUI.activeSelf);
    }

    private void OnGUI()
    {
        coinText.SetText("Coins: " + currentCoins);
    }
}

[System.Serializable]
public class Item {
    public string itemName;
    public int cost;
    public Sprite image;
    public GameObject itemPrefab;
    [HideInInspector] public GameObject itemRef;
    [HideInInspector] public int quantity;
}


