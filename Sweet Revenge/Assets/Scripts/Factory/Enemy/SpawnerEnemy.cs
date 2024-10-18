using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [Header("Spawn Parameters")]
    [SerializeField] private FactoryEnemy enemyFactory;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] List<string> enemyTypeToSpawn = new List<string>();
    [SerializeField] private int enemiesPerSpawn = 3;

    [Header("Rondas")]
    [SerializeField] private int currentRound = 1;
    private int enemiesPerRound;

    
    private void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
    }
   
    private IEnumerator SpawnEnemiesRoutine()
    {
        yield return new WaitForSeconds(spawnInterval);
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        string enemyType = enemyTypeToSpawn[UnityEngine.Random.Range(0, enemyTypeToSpawn.Count)];
        SpawnEnemies(enemyType, spawnPoint.position);
        
    }
    private void SpawnEnemies(string enemyType, Vector3 position)
    {
        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            Enemy newEnemy = enemyFactory.CreateEnemy(enemyType);
            newEnemy.transform.position = position;
        }
    }
}
