using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        DontDestroyOnLoad(gameObject);
        }

    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }
    public void WinGame()
    {
        SceneManager.LoadScene("Win");
      
    }
    public void LoseGame()
    {
        SceneManager.LoadScene("Lose");
      
    }
}
