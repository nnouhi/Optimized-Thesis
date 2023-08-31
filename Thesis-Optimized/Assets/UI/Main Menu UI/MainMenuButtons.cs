using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MainMenuButtons : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button startSpeedrunButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button previousPageButton;
    [SerializeField] private Button mainMenuButtonHowToPlay;
    [SerializeField] private Button mainMenuButtonSettings;
    [SerializeField] private Button settingsButton;

    [Header("Canvases")]
    [SerializeField] private GameObject[] canvases;

    [Header("How To Play Text")]
    [SerializeField] private TextMeshProUGUI howToPlayText;
    [SerializeField] [TextArea] private string[] howToPlayTextString;

    private int currentPageIndex = 0;
    private int howToPlayTextIndex = 0;

    void Start()
    {
        startButton.onClick.AddListener(() => StartGame(SceneManager.GetActiveScene().buildIndex + 1));
        startSpeedrunButton.onClick.AddListener(() => StartGame(SceneManager.GetActiveScene().buildIndex + 2));
        quitButton.onClick.AddListener(() => QuitGame());
        howToPlayButton.onClick.AddListener(() => ShowCanvas(0, 1));
        settingsButton.onClick.AddListener(() => ShowCanvas(0, 3));
        mainMenuButtonHowToPlay.onClick.AddListener(() => ShowCanvas(/*canvases.Length - 1*/ 2, 0));
        mainMenuButtonSettings.onClick.AddListener(() => ShowCanvas(/*canvases.Length - 1*/ 3, 0));
        nextPageButton.onClick.AddListener(() => NextPage());
        previousPageButton.onClick.AddListener(() => PreviousPage());

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;        
        Cursor.visible = true;

        GameManager.Instance.TargetFrameRate = 60;

        #if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
        #else
            Debug.unityLogger.logEnabled = false;
        #endif
    }

    private void StartGame(int scene)
    {
        // Load the first level
        SceneManager.LoadScene(scene);
    }

    private void QuitGame()
    {
        // Quit the game
    #if UNITY_STANDALONE
        Application.Quit();
    #endif
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
    
    private void ShowCanvas(int currentPageIndex, int nextPageIndex)
    {
        // Show the how to play canvas
        canvases[currentPageIndex].SetActive(false);
        canvases[nextPageIndex].SetActive(true);

        this.currentPageIndex = nextPageIndex;
    }

    private void PreviousPage()
    {
        if (currentPageIndex == 2)
        {
            howToPlayText.text = howToPlayTextString[--howToPlayTextIndex];
            currentPageIndex--;
        }
        else if (currentPageIndex == 1)
        {
            ShowCanvas(1, 0);
        }
        else if (currentPageIndex == canvases.Length - 1)
        {
            ShowCanvas(canvases.Length - 1, 0);
        }
       
    }

    private void NextPage()
    {
        if (currentPageIndex == 1)
        {
            howToPlayText.text = howToPlayTextString[++howToPlayTextIndex];
            currentPageIndex++;
        }
        else if (currentPageIndex == 2)
        {
            howToPlayTextIndex = 0;
            howToPlayText.text = howToPlayTextString[howToPlayTextIndex];
            ShowCanvas(1, 2);
        }
    }
}
