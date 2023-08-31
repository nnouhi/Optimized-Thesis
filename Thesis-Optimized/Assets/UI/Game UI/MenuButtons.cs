using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    [Header("Game Over Canvas Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [Header("Game Pause Canvas Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButtonGP;
    [SerializeField] private Button mainMenuButtonGP;
    [SerializeField] private Button resetNumberOfTriesButton;

    [Header("Canvases")]
    [SerializeField] private Canvas gamePauseCanvas;
    [SerializeField] private Canvas guiCanvas;


    [Header("Ran Out Of Time Canvas Buttons")]
    [SerializeField] private Button restartButtonRanOutOfTime;
    [SerializeField] private Button quitButtonRanOutOfTime;

    [Header("You Won Canvas Buttons")]
    [SerializeField] private Button restartButtonYouWon;
    [SerializeField] private Button quitButtonYouWon;


    void Start()
    {
        AddListeners();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        Debug.Log(Time.timeScale);

        // if (SceneManager.GetActiveScene().buildIndex == 1)
        // {
        //     resetNumberOfTriesButton.gameObject.SetActive(false);
        // }
    }

    private void AddListeners()
{
        restartButton.onClick.AddListener(() => RestartGame());
        restartButtonRanOutOfTime.onClick.AddListener(() => RestartGame());
        restartButtonYouWon.onClick.AddListener(() => RestartGame());
        restartButtonGP.onClick.AddListener(() => RestartGame());
        quitButton.onClick.AddListener(() => QuitGame());
        quitButtonRanOutOfTime.onClick.AddListener(() => QuitGame());
        quitButtonYouWon.onClick.AddListener(() => QuitGame());
        continueButton.onClick.AddListener(() => ContinueGame());
        mainMenuButtonGP.onClick.AddListener(() => BackToMainMenu());
        resetNumberOfTriesButton.onClick.AddListener(() => ResetNumberOfTries());
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        RemoveListeners();
    }

    private void RemoveListeners()
    {
        restartButton.onClick.RemoveListener(() => RestartGame());
        restartButtonRanOutOfTime.onClick.RemoveListener(() => RestartGame());
        restartButtonYouWon.onClick.RemoveListener(() => RestartGame());
        restartButtonGP.onClick.RemoveListener(() => RestartGame());
        quitButton.onClick.RemoveListener(() => QuitGame());
        quitButtonRanOutOfTime.onClick.RemoveListener(() => QuitGame());
        quitButtonYouWon.onClick.RemoveListener(() => QuitGame());
        continueButton.onClick.RemoveListener(() => ContinueGame());
        mainMenuButtonGP.onClick.RemoveListener(() => BackToMainMenu());
        resetNumberOfTriesButton.onClick.RemoveListener(() => ResetNumberOfTries());

    }

    private void QuitGame()
    {
    #if UNITY_STANDALONE
        Application.Quit();
    #endif
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Unpause game that was paused in DeathHandler
        Time.timeScale = 1;
        RemoveListeners();
    }

    public void ResetNumberOfTries()
    {
       PlayerPerformanceManager.Instance.ResetPlayerStats(true);
       RestartGame();
    }


    private void ContinueGame()
    {
        gamePauseCanvas.enabled = false;
        guiCanvas.enabled = true;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;        
        Cursor.visible = false;
        FindObjectOfType<WeaponSwitcher>().enabled = true;
        WeaponZoom weaponZoom = FindObjectOfType<WeaponZoom>();
        if (weaponZoom)
        {
            weaponZoom.enabled = true;
        }  
    }    
}
