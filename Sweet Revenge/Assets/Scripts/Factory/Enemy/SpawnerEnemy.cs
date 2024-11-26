using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [Header("Spawn Parameters")]
    [SerializeField] private FactoryEnemy enemyFactory;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private List<SpawnPoint> spawnPoints; // Lista de puntos de spawn
    [SerializeField] private List<string> enemyTypeToSpawn = new List<string>();
    [SerializeField] private int enemiesPerSpawn = 1;

    [Header("Rounds")]
    [SerializeField] private int currentRound = 0;
    [SerializeField] private int baseEnemiesPerRound = 2;
    [SerializeField] private int roundMultiplier = 2;
    [SerializeField] private float timeBetweenRounds = 5f; // Wait time between rounds
    [SerializeField] private int maxRounds = 15; // Máximo número de rondas
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

    [SerializeField] private TextMeshProUGUI roundText;
   
    private SpawnPoint.SpawnArea currentArea; // Área actual

    private Dictionary<SpawnPoint.SpawnArea, List<SpawnPoint.SpawnArea>> areaTransitions = new Dictionary<SpawnPoint.SpawnArea, List<SpawnPoint.SpawnArea>>();

    private void Awake()
    {
        // Initialize area transitions
        InitializeAreaTransitions();
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
        UpdateRoundText();

        if (currentRound <= maxRounds)
        {
            if (isSpawning)
            {
                spawnTimer += Time.deltaTime;
                if (spawnTimer >= spawnInterval && spawnCount < CalculateEnemiesPerRound())
                {
                    SpawnEnemies();
                    spawnTimer = 0f;
                }

                if (activeEnemies == 0 && spawnCount >= CalculateEnemiesPerRound() && spawnCount > 0)
                {
                    isSpawning = false;
                    roundTimer = 0f;
                    Debug.Log($"Round {currentRound} completed.");
                }
            }
            else
            {
                roundTimer += Time.deltaTime;
                if (roundTimer >= timeBetweenRounds)
                {
                    RewardPlayer(CalculateCoinsReward(currentRound));
                    currentRound++;
                    spawnCount = 0;
                    isSpawning = true;
                }
            }
        }
        else
        {
            Debug.Log("Maximum number of rounds reached.");
        }
    }

    #region Rounds
    private void SpawnEnemies()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.IsActive)
            {
                for (int i = 0; i < enemiesPerSpawn; i++)
                {
                    string enemyType = enemyTypeToSpawn[Random.Range(0, enemyTypeToSpawn.Count)];
                    Enemy newEnemy = enemyFactory.CreateEnemy(enemyType);
                    newEnemy.transform.position = spawnPoint.GetSpawnPosition();
                    activeEnemies++;
                    newEnemy.enemyHealth.OnDeath += OnEnemyDeath;
                    Debug.Log($"Spawned enemy: {enemyType}. Active enemies: {activeEnemies}");
                }
                spawnCount++;
            }
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
    #endregion

    #region Spawn Management
    private void InitializeAreaTransitions()
    {
        areaTransitions[SpawnPoint.SpawnArea.Start] = new List<SpawnPoint.SpawnArea>
        {
            SpawnPoint.SpawnArea.Cueva,
            SpawnPoint.SpawnArea.Volcan,
            SpawnPoint.SpawnArea.Tierra
        };
        areaTransitions[SpawnPoint.SpawnArea.Cueva] = new List<SpawnPoint.SpawnArea>
        {
            SpawnPoint.SpawnArea.Cueva_Profunda,
            SpawnPoint.SpawnArea.Start
        };
        areaTransitions[SpawnPoint.SpawnArea.Cueva_Profunda] = new List<SpawnPoint.SpawnArea>
        {
            SpawnPoint.SpawnArea.Cueva
        };
        areaTransitions[SpawnPoint.SpawnArea.Volcan] = new List<SpawnPoint.SpawnArea>
        {
            SpawnPoint.SpawnArea.Start
        };
        areaTransitions[SpawnPoint.SpawnArea.Tierra] = new List<SpawnPoint.SpawnArea>
        {
            SpawnPoint.SpawnArea.Start
        };
    }
    private void ActivateSpawnArea(SpawnPoint.SpawnArea current, SpawnPoint.SpawnArea next)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.Area == current)
            {
                spawnPoint.DisableSpawn();
            }
            else if (spawnPoint.Area == next)
            {
                spawnPoint.EnableSpawn();
            }
        }
        currentArea = next; // Actualiza el área actual
    }

    private SpawnPoint.SpawnArea GetNextArea(SpawnPoint.SpawnArea current)
    {
        // Randomly select the next area from the possible transitions
        if (areaTransitions.ContainsKey(current))
        {
            List<SpawnPoint.SpawnArea> possibleNextAreas = areaTransitions[current];
            return possibleNextAreas[Random.Range(0, possibleNextAreas.Count)];
        }
        return current; // If no transitions, stay in the current area
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the next area based on the current area
            SpawnPoint.SpawnArea nextArea = GetNextArea(currentArea);
            ActivateSpawnArea(currentArea, nextArea);
        }
    }
    #endregion
}