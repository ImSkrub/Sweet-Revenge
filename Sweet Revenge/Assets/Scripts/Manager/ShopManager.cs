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
    //[Header("Chest")]
    //[SerializeField] private GameObject chestPrefab; // Reference to the chest prefab
    //private Chest chestInstance; // Reference to the chest instance


    // Reference to the spawn point
    [SerializeField] private Transform itemSpawnPoint;
    private List<Vector3> occupiedPositions = new List<Vector3>(); // List to track occupied positions

    public bool itemsSpawned = false;
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
        //chestInstance = FindObjectOfType<Chest>();
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
            if (!item.isPermanent)
            {
                SpawnPurchasedItems(item); // Pass the item to spawn
            }
            else
            {
                ApplyItem(item);
            }
        }
    }
    
    public void ApplyItem(Item item)
    {
        switch (item.itemName)
        {
            case "Life Up":
                player.SetNewMaxHealth(25);
                break;
            case "Dmg up":
                player.SetNewMaxDamage(25);
                break;
            case "Stamina Up":
                player.SetNewMaxStamina(25); break;

        }
    }
    
    private void SpawnPurchasedItems(Item item)
    {
        // Calculate a spawn position
        Vector3 spawnPosition = GetNextSpawnPosition();

        if (spawnPosition != Vector3.zero) // Check if a valid position was found
        {
            // Instantiate the item prefab at the calculated position
            GameObject spawnedItem = Instantiate(item.itemPrefab, spawnPosition, Quaternion.identity);
            // Add the position to the occupied list
            occupiedPositions.Add(spawnPosition);
            Debug.Log($"Spawned {item.itemName} at {spawnPosition}");
        }
        else
        {
            Debug.LogWarning("No available spawn positions!");
        }

        item.quantity = 0; // Reset quantity after spawning
    }
    /*
    //itemsSpawned = true; // Mark that items were spawned
    // Notify the chest to spawn the item
    // chestInstance.SpawnItem(item.itemPrefab); // Commented out as per request
    // If items were spawned, allow the chest to be opened again
    // if (itemsSpawned) { chestInstance.SetCanOpen(true); } // Commented out as per request
    */
    private Vector3 GetNextSpawnPosition()
    {
        // Define the spacing between items
        float spacing = 1.0f; // Adjust this value as needed
        Vector3 basePosition = itemSpawnPoint.position;

        // Check for the next available position
        for (int i = 0; i < 100; i++) // Limit the number of attempts to find a position
        {
            Vector3 newPosition = basePosition + new Vector3((i % 10) * spacing, (i / 10) * spacing, 0); // Create a grid-like pattern

            // Check if the position is already occupied
            if (!occupiedPositions.Contains(newPosition))
            {
                return newPosition; // Return the first unoccupied position found
            }
        }

        return Vector3.zero; // Return zero if no position is found
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
    public bool isPermanent = false;
    public int cost;
    public Sprite image;
    public GameObject itemPrefab;
    [HideInInspector] public GameObject itemRef;
    [HideInInspector] public int quantity;
}


