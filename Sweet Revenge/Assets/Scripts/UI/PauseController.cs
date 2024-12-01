using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    
    public bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)||(Input.GetKeyDown(KeyCode.Escape)))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame(); 
            }
        }
    }


    void OnEnable()
    {
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void PauseGame()
    {
        HandlePauseUI();
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        HandlePauseUI();
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void HandlePauseUI()
    {
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
    }

    public void OnContinueButton()
    {
        ResumeGame();
    }
}
