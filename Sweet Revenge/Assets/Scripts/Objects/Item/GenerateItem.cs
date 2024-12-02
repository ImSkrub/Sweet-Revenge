using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GenerateItem : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab, coinPrefabDoor; // Prefab of the coin
    [SerializeField] private int numberOfCoins = 3; // Number of coins to spawn
    [SerializeField] private float spawnOffset = 0.5f; // Offset for spawning coins

    public void SpawnItem()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            // Calculate a random offset for each coin to avoid overlap
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0);
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            Instantiate(coinPrefabDoor, spawnPosition, Quaternion.identity);
        }
    }
}
