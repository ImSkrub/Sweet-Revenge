using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [Header("Spawn Parameters")]
    [SerializeField] private FactoryEnemy enemyFactory;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float updateInterval = 2f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private List<string> enemyTypeToSpawn = new List<string>();
    [SerializeField] private int enemiesPerSpawn = 1;

    [Header("Rounds")]
    [SerializeField] private int currentRound = 1;
    [SerializeField] private int baseEnemiesPerRound = 2;
    [SerializeField] private int baseCoinReward = 100;
    [SerializeField] private int roundMultiplier = 2;
    [SerializeField][Range(1,2)] private float coinMultiplier = 1.5f;
    [SerializeField] private float timeBetweenRounds = 5f; // Wait time between rounds

    public bool activateSpawn = true;
    private int spawnCount = 0;
    private int activeEnemies = 0;
    private void FixedUpdate()
    {
        if(activateSpawn && !IsInvoking("StartSpawn"))
        {
            Invoke("StartSpawn", 0f);
        }
    }
    private void StartSpawn()
    {
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        yield return new WaitForSeconds(updateInterval);

        // Calcular el número de enemigos por ronda
        int enemiesPerRound = CalculateEnemiesPerRound();

        // Reiniciar el conteo de enemigos activos y spawnCount al inicio de la ronda
        activeEnemies = 0; // Reiniciar el conteo de enemigos activos
        spawnCount = 0; // Reiniciar el contador de enemigos para la nueva ronda

        while (activateSpawn && spawnCount < enemiesPerRound)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Generar enemigos hasta alcanzar el límite de enemigos por ronda
            for (int i = 0; i < enemiesPerSpawn && spawnCount < enemiesPerRound; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                string enemyType = enemyTypeToSpawn[Random.Range(0, enemyTypeToSpawn.Count)];
                SpawnEnemies(enemyType, spawnPoint.position);
                spawnCount++;
            }

            // Esperar hasta que todos los enemigos activos sean derrotados
            yield return new WaitUntil(() => activeEnemies == 0);

            //// Recompensar al jugador después de cada ronda
            //int coinsRewarded = CalculateCoinsReward(currentRound);
            //RewardPlayer(coinsRewarded);

            // Esperar antes de comenzar la siguiente ronda
            yield return new WaitForSeconds(timeBetweenRounds);
            currentRound++;
        }

        // Reiniciar la activación de spawn si no hay enemigos
        if (spawnCount == 0) activateSpawn = true;
    }
    private int CalculateEnemiesPerRound()
    {
        return baseEnemiesPerRound + (currentRound - 1) * roundMultiplier;

    }

    private int CalculateCoinsReward(int round)
    {
        int reward = Mathf.RoundToInt(baseCoinReward * Mathf.Pow(coinMultiplier, round));// 100 coins for round 1, 300 for round 2, etc.
        return reward;
    }

    private void RewardPlayer(int coins)
    {
        // Implement your coin reward logic here
        PointManager.Instance.AddShopCoin(coins);
        Debug.Log($"Player rewarded with {coins} coins for completing round {currentRound}.");
    }

    private void SpawnEnemies(string enemyType, Vector3 position)
    {
        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            Enemy newEnemy = enemyFactory.CreateEnemy(enemyType);
            newEnemy.transform.position = position;
            activeEnemies++; // Incrementar el conteo de enemigos activos
            newEnemy.enemyHealth.OnDeath += OnEnemyDeath; // Suscribirse al evento de muerte del enemigo
        }
    }

    private void OnEnemyDeath()
    {
        activeEnemies--;
        Debug.Log("Enemy died. Active enemies: " + activeEnemies);
    }
}
