using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPowerUp : MonoBehaviour
{
    [SerializeField] private FactoryPowerUp powerUpFactory;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] List<string> powerUpTypes = new List<string>();

    void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps()
    {
        while (true)
        {

            yield return new WaitForSeconds(spawnInterval);
            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

            string randomPowerUpType = powerUpTypes[UnityEngine.Random.Range(0, powerUpTypes.Count)];
            Debug.Log($"Spawn point position: {spawnPoint.position}");
            SpawnPowerUpAtPosition(randomPowerUpType, spawnPoint.position);

        }
    }

    private void SpawnPowerUpAtPosition(string powerUpType, Vector3 position)
    {
        IPowerUp powerUp = powerUpFactory.CreatePowerUp(powerUpType);
      
        if (powerUp is MonoBehaviour powerUpMono && powerUp != null)
        {
            powerUpMono.transform.position = position;  // Coloca el power-up en la posición dada
            powerUpMono.gameObject.SetActive(true);
            Debug.Log($"Power-up '{powerUpType}' spawned at {position}");
        }
             
        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach (Transform spawnPoint in spawnPoints)
        {
            Gizmos.DrawSphere(spawnPoint.position, 0.5f);
        }
    }
}
