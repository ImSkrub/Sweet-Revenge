using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private AudioClip buyItemSoundClip, declineBuySoundClip;
    public Transform shopContent;
    public GameObject itemPrefab;
    private Player player;
    private int currentCoins = 0;


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
      
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        // Initialize the shop items
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
        // Check if the player has enough coins using PointManager
        if (PointManager.Instance._shopCoin >= item.cost)
        {
            // Check if the item is permanent
            if (item.isPermanent)
            {
                // Check if the item has reached its purchase limit
                if (item.currentPurchaseCount < item.maxPurchaseLimit)
                {
                    // Deduct the cost from PointManager
                    PointManager.Instance.AddShopCoin(-item.cost); // Deduct coins
                    item.quantity++;
                    item.itemRef.transform.GetChild(0).GetComponent<TMP_Text>().SetText(item.quantity.ToString());
                    ApplyItem(item); // Apply the item directly
                    item.currentPurchaseCount++; // Increment the purchase count
                    SoundFXManager.instance.PlaySoundFXClip(buyItemSoundClip, transform, 1f);
                }
                else
                {
                    Debug.LogWarning($"Cannot purchase {item.itemName}. Maximum purchase limit reached.");
                    SoundFXManager.instance.PlaySoundFXClip(declineBuySoundClip, transform, 1f);
                }
            }
            else
            {
                // Deduct the cost from PointManager
                PointManager.Instance.AddShopCoin(-item.cost); // Deduct coins
                item.quantity++;
                item.itemRef.transform.GetChild(0).GetComponent<TMP_Text>().SetText(item.quantity.ToString());
                SpawnPurchasedItems(item); // Pass the item to spawn
                SoundFXManager.instance.PlaySoundFXClip(buyItemSoundClip, transform, 1f);
            }
        }
        else
        {
            SoundFXManager.instance.PlaySoundFXClip(declineBuySoundClip, transform, 1f);
        }
    }

    public void ApplyItem(Item item)
    {
        // Check if the item has already been applied the maximum number of times
        if (item.currentPurchaseCount < item.maxPurchaseLimit)
        {
            switch (item.itemName)
            {
                case "Life Up":
                    player.SetNewMaxHealth(25);
                    break;
                case "Dmg Up":
                    player.SetNewMaxDamage(25);
                    break;
                case "Stamina Up":
                    player.SetNewMaxStamina(25);
                    break;
            }
            item.currentPurchaseCount++; // Increment the purchase count after applying
            Debug.Log($"Applied permanent item: {item.itemName}");
        }
        else
        {
            Debug.LogWarning($"Cannot apply {item.itemName}. Maximum application limit reached.");
        }
    }

    private void SpawnPurchasedItems(Item item)
    {
        // Check if the item should be spawned based on its permanence
        if (!item.isPermanent) // Only spawn if the item is not permanent
        {
            // Check if the itemPrefab is assigned
            if (item.itemPrefab == null)
            {
                Debug.LogError($"Item prefab for {item.itemName} is not assigned.");
                return; // Exit the method if the prefab is not assigned
            }

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
        else
        {
            // If the item is permanent, apply it directly without spawning
            ApplyItem(item);
            Debug.Log($"Applied permanent item: {item.itemName}");
        }
    }

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
}

[System.Serializable]
public class Item {
    public string itemName;
    public bool isPermanent = false;
    public int cost;
    public int maxPurchaseLimit = 4;
    public Sprite image;
    public GameObject itemPrefab;
    [HideInInspector] public GameObject itemRef;
    [HideInInspector] public int quantity;
    [HideInInspector] public int currentPurchaseCount = 0;
}