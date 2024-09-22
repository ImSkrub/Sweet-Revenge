using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private PlayerController player;
    [SerializeField] private Pistol pistol;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Space))
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

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        player.canRotate = false;
        pistol.canRotate = false;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        player.canRotate = true;
        pistol.canRotate = true;
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void OnContinueButton()
    {
        ResumeGame();
    }
}
