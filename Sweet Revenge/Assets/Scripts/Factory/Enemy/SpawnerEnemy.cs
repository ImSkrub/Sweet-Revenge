using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [Header("Spawn Parameters")]
    [SerializeField] private FactoryEnemy enemyFactory;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private List<string> enemyTypeToSpawn = new List<string>();
    [SerializeField] private int enemiesPerSpawn = 1;

    [Header("Rounds")]
    [SerializeField] private int currentRound = 1;
    [SerializeField] private int baseEnemiesPerRound = 2;
    [SerializeField] private int roundMultiplier = 2;
    [SerializeField] private float timeBetweenRounds = 5f; // Wait time between rounds
    [SerializeField] private int maxRounds = 15; // Máximo número de rondas

    private int spawnCount = 0;
    private int activeEnemies = 0;

    private void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (currentRound <= maxRounds) // Loop hasta que se alcance el número máximo de rondas
        {
            // Calcular el número de enemigos por ronda
            int enemiesPerRound = CalculateEnemiesPerRound();
            spawnCount = 0; // Reiniciar el contador de enemigos por ronda
            activeEnemies = 0; // Reiniciar el conteo de enemigos activos

            // Generar enemigos hasta alcanzar el límite de enemigos por ronda
            while (spawnCount < enemiesPerRound)
            {
                yield return new WaitForSeconds(spawnInterval);

                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                string enemyType = enemyTypeToSpawn[Random.Range(0, enemyTypeToSpawn.Count)];
                SpawnEnemies(enemyType, spawnPoint.position, enemiesPerSpawn);
            }

            // Esperar hasta que todos los enemigos activos sean derrotados
            yield return new WaitUntil(() => activeEnemies == 0);

            // Recompensar al jugador después de cada ronda
            int coinsRewarded = CalculateCoinsReward(currentRound);
            RewardPlayer(coinsRewarded);

            // Esperar antes de comenzar la siguiente ronda
            yield return new WaitForSeconds(timeBetweenRounds);
            currentRound++;
        }

        // Aquí puedes agregar lógica para lo que sucede después de alcanzar el número máximo de rondas
        Debug.Log("Se ha alcanzado el número máximo de rondas.");
    }

    private int CalculateEnemiesPerRound()
    {
        return baseEnemiesPerRound + (currentRound - 1) * roundMultiplier;
    }

    private int CalculateCoinsReward(int round)
    {
        int reward = Mathf.RoundToInt(100 * Mathf.Pow(1.5f, round)); // 100 coins for round 1, etc.
        return reward;
    }

    private void RewardPlayer(int coins)
    {
        PointManager.Instance.AddShopCoin(coins);
        Debug.Log($"Player rewarded with {coins} coins for completing round {currentRound}.");
    }

    private void SpawnEnemies(string enemyType, Vector3 position, int enemiesPerSpawn)
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
