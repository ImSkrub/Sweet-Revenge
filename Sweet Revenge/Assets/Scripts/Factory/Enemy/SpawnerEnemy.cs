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
    private int spawnCount = 0;

    [Header("Rondas")]
    [SerializeField] private int currentRound = 1;
    [SerializeField] private int enemiesPerRound = 10;
    [SerializeField] private int roundMultiplier = 2;
    [SerializeField] private float timeBetweenRounds = 5f; //wait time between rounds
        
    private void Start()
    {
        enemiesPerRound = currentRound * roundMultiplier;
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private void Update()
    {
        if(enemiesPerRound >= spawnCount)
        {
            currentRound++;
            spawnCount = 0;
        }
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        //agregar un while para meterle un valor de cuando tiene que parar, por ejemplo cuando salga del area que pare de spawnear.
        spawnCount = 0;
        enemiesPerRound = currentRound * roundMultiplier;
        while(spawnCount < enemiesPerRound)
        {
            yield return new WaitForSeconds(spawnInterval);
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            string enemyType = enemyTypeToSpawn[Random.Range(0, enemyTypeToSpawn.Count)];
            SpawnEnemies(enemyType, spawnPoint.position);
            spawnCount += enemiesPerSpawn;
        }
        yield return new WaitForSeconds(timeBetweenRounds);
        currentRound++;
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
