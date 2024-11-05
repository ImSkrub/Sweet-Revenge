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
    [SerializeField] private int currentRound = 0;
    [SerializeField] private int baseEnemiesPerRound = 2;
    [SerializeField] private int roundMultiplier = 2;
    [SerializeField] private float timeBetweenRounds = 5f; // Wait time between rounds
    [SerializeField] private int maxRounds = 15; // Máximo número de rondas

    private int spawnCount = 0;
    private int activeEnemies = 0;
    private float spawnTimer = 0f; // Temporizador para el intervalo de spawn
    private float roundTimer = 0f; // Temporizador para el tiempo entre rondas
    private bool isSpawning = false; // Indica si se están generando enemigos

    private void Update()
    {
        if (currentRound <= maxRounds)
        {
            if (isSpawning)
            {
                // Manejar el temporizador de spawn
                spawnTimer += Time.deltaTime;
                if (spawnTimer >= spawnInterval && spawnCount < CalculateEnemiesPerRound())
                {
                    SpawnEnemies();
                    spawnTimer = 0f; // Reiniciar el temporizador
                }

                // Verificar si todos los enemigos han sido derrotados
                if (activeEnemies == 0 && spawnCount >= CalculateEnemiesPerRound() && spawnCount > 0)
                {
                    isSpawning = false; // Detener la generación de enemigos
                    roundTimer = 0f; // Reiniciar el temporizador de ronda
                    Debug.Log($"Round {currentRound} completed.");
                }
            }
            else
            {
                // Manejar el temporizador entre rondas
                roundTimer += Time.deltaTime;
                if (roundTimer >= timeBetweenRounds)
                {
                    // Recompensar al jugador y avanzar a la siguiente ronda
                    int coinsRewarded = CalculateCoinsReward(currentRound);
                    RewardPlayer(coinsRewarded);
                    currentRound++;
                    spawnCount = 0; // Reiniciar el contador de enemigos por ronda
                    isSpawning = true; // Comenzar a generar enemigos
                }
            }
        }
        else
        {
            // Aquí puedes agregar lógica para lo que sucede después de alcanzar el número máximo de rondas
            Debug.Log("Se ha alcanzado el número máximo de rondas.");
        }
    }


    private void SpawnEnemies()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        string enemyType = enemyTypeToSpawn[Random.Range(0, enemyTypeToSpawn.Count)];
        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            Enemy newEnemy = enemyFactory.CreateEnemy(enemyType);
            newEnemy.transform.position = spawnPoint.position;
            activeEnemies++; // Incrementar el conteo de enemigos activos
            newEnemy.enemyHealth.OnDeath += OnEnemyDeath; // Suscribirse al evento de muerte del enemigo
            Debug.Log($"Spawned enemy: {enemyType}. Active enemies: {activeEnemies}");
        }
        spawnCount++; // Incrementar el contador de enemigos generados
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

    private void OnEnemyDeath()
    {
        activeEnemies--;
        Debug.Log("Enemy died. Active enemies: " + activeEnemies);
    }
}
