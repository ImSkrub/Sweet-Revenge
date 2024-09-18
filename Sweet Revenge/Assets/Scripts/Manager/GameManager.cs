using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
     DontDestroyOnLoad(gameObject);
       //event in case of death
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
