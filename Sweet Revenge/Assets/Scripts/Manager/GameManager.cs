using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlayerLife player;
    private SpawnerEnemy spawnerEnemy;
    private GameStatusManager quickRevive;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        player = FindObjectOfType<PlayerLife>();
        player.OnDeath += HandlePlayerDeath;
        spawnerEnemy = FindObjectOfType<SpawnerEnemy>();
        quickRevive = FindObjectOfType<GameStatusManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        if(spawnerEnemy.CurrentRound >= 15)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
       SceneManager.LoadScene(3);
    }

    private void HandlePlayerDeath()
    {
        if (quickRevive.HasSavedStates())
        {
            quickRevive.Checkpoint(); // Restore the last saved state
            Debug.Log("Player restored from checkpoint.");
        }
        else
        {
            LoseGame(); // No saved states, proceed to lose game
        }
    }

    public void LoseGame()
    {
        spawnerEnemy.CurrentRound = 0;
        SceneManager.LoadScene(2);
    }


}
