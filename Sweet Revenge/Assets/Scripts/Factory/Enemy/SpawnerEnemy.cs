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
    [SerializeField] private int enemiesPerSpawn = 3;

    [Header("Rounds")]
    [SerializeField] private int currentRound = 1;
    [SerializeField] private int baseEnemiesPerRound = 10;
    [SerializeField] private int baseCoinReward = 100;
    [SerializeField] private int roundMultiplier = 2;
    [SerializeField][Range(1,2)] private float coinMultiplier = 1.5f;
    [SerializeField] private float timeBetweenRounds = 5f; // Wait time between rounds

    private int spawnCount = 0;
    private int activeEnemies = 0;
    private void Start()
    {
      StartCoroutine(SpawnEnemiesRoutine());
    }
    private IEnumerator SpawnEnemiesRoutine()
    {
        while (true) // Infinite loop to keep spawning rounds, add a variable for it to stop.
        {
            spawnCount = 0;
            int enemiesPerRound = CalculateEnemiesPerRound();
            while (spawnCount <= enemiesPerRound)
            {
                yield return new WaitForSeconds(spawnInterval);
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                string enemyType = enemyTypeToSpawn[Random.Range(0, enemyTypeToSpawn.Count)];
                SpawnEnemies(enemyType, spawnPoint.position);
                spawnCount += enemiesPerSpawn;
            }

            yield return new WaitUntil(() => activeEnemies == 0);

            // Reward the player after each round
            int coinsRewarded = CalculateCoinsReward(currentRound);
            RewardPlayer(coinsRewarded);

            // Wait before starting the next round
            yield return new WaitForSeconds(timeBetweenRounds);
            currentRound++;
        }
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
            activeEnemies++; // Increment active enemies count
            newEnemy.OnDeath += OnEnemyDeath; // Subscribe to the enemy's death event
        }
    }

    private void OnEnemyDeath()
    {
        activeEnemies--;
    }
}
