using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private TextMeshProUGUI roundText;
    
    public int CurrentRound
    {
        get => currentRound;
        set => currentRound = Mathf.Max(0, value); // Setter with validation
    }

    private int spawnCount = 0;
    private int activeEnemies = 0;
    private float spawnTimer = 0f; // Temporizador para el intervalo de spawn
    private float roundTimer = 0f; // Temporizador para el tiempo entre rondas
    private bool isSpawning = false; // Indica si se están generando enemigos

    private void Awake()
    {
        // Automatically find the TextMeshProUGUI component with the tag "RoundText"
        GameObject roundTextObject = GameObject.FindGameObjectWithTag("RoundText");
        if (roundTextObject != null)
        {
            roundText = roundTextObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("No GameObject with tag 'RoundText' found in the scene.");
        }

        // Optionally, check if the roundText was found
        if (roundText == null)
        {
            Debug.LogError("RoundText TMP component not found in the GameObject with tag 'RoundText'. Please ensure it exists.");
        }
    }

    private void Update()
    {
        // Update the round text in the HUD
        UpdateRoundText();

        if (currentRound <= maxRounds)
        {
            if (isSpawning)
            {
                // Handle spawn timer
                spawnTimer += Time.deltaTime;
                if (spawnTimer >= spawnInterval && spawnCount < CalculateEnemiesPerRound())
                {
                    SpawnEnemies();
                    spawnTimer = 0f; // Reset timer
                }

                // Check if all enemies have been defeated
                if (activeEnemies == 0 && spawnCount >= CalculateEnemiesPerRound() && spawnCount > 0)
                {
                    isSpawning = false; // Stop spawning enemies
                    roundTimer = 0f; // Reset round timer
                    Debug.Log($"Round {currentRound} completed.");
                }
            }
            else
            {
                // Handle round timer
                roundTimer += Time.deltaTime;
                if (roundTimer >= timeBetweenRounds)
                {
                    // Reward the player and advance to the next round
                    int coinsRewarded = CalculateCoinsReward(currentRound);
                    RewardPlayer(coinsRewarded);
                    currentRound++;
                    spawnCount = 0; // Reset enemy count for the round
                    isSpawning = true; // Start spawning enemies
                }
            }
        }
        else
        {
            // Logic for what happens after reaching the maximum number of rounds
            Debug.Log("Maximum number of rounds reached.");
        }
    }

    private void UpdateRoundText()
    {
        // Update the TMP text to show the current round
        if (roundText != null)
        {
            roundText.text = $"Round: {currentRound}"; // Update the text
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
