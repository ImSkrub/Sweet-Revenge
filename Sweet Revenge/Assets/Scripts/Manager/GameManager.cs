using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
  


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
        //player.GetComponent<LifePlayer>().OnDeath += FinishGame;
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
       // AudioManager.instance.PlaySound(7);
        SceneManager.LoadScene(14);
       //PointManager.Instance.SaveFinalScore();
    }

    public void FinishGame()
    {
       // AudioManager.instance.PlaySound(6);
        SceneManager.LoadScene(12);
        //PointManager.Instance.SaveFinalScore();
    }

   
}
