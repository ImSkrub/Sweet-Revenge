using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerLife player;
    public int eDeathCount;


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
        
        player.GetComponent<PlayerLife>().onDeath += FinishGame;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        if (eDeathCount >= 3)
        {
            CompleteGame();
        }


    }
    //Ganar
    public void CompleteGame()
    {
        // AudioManager.instance.PlaySound(7);
        SceneManager.LoadScene(4);
       //PointManager.Instance.SaveFinalScore();
    }
    //Perder
    public void FinishGame()
    {
       // AudioManager.instance.PlaySound(6);
        SceneManager.LoadScene(3);
        //PointManager.Instance.SaveFinalScore();
    }

   
}
