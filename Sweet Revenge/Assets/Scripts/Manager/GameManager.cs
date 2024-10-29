using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlayerLife player;


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
        player.OnDeath += FinishGame;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        
        
    }

    public void CompleteGame()
    {
       
        SceneManager.LoadScene(14);
       
    }

    public void FinishGame()
    {
       
        SceneManager.LoadScene(12);
       
    }

   
}
