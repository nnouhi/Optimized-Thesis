using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UndergroundBehavior : MonoBehaviour
{
    [SerializeField] private Canvas undergroundCanvas;
    [SerializeField] private Canvas guiCanvas;
    [SerializeField] private Canvas wonGameCanvas;
    [SerializeField] private Canvas ranOutOfTimeCanvas;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI spawnersText;
    [SerializeField] private TextMeshProUGUI killedZombiesText;
    [SerializeField] private TextMeshProUGUI aliveZombiesText;
    [SerializeField] private TextMeshProUGUI winnerCanvasTimeLeftText;
    [SerializeField] private TextMeshProUGUI undergroundPromptText;
    [SerializeField] private float remainingTimeInSeconds = 120.0f;
    [SerializeField] private Button readyButton;
    private TimeSpan timeSpan;
    private bool wasCanvasDisplayed = false;
    public bool WasCanvasDisplayed { get => wasCanvasDisplayed; set => wasCanvasDisplayed = value; }
    private int numberOfKilledZombies = 0;
    private int numberOfAliveZombies = 0;
    private int numberOfSpawners = 0;
    private int maxnumberOfSpawners = 0;
    private float startingTimeRemainingInSeconds = 0.0f;
    private bool isGameFinished = false;
    private void Awake()
    {
        readyButton.onClick.AddListener(() => HideUndergroundCanvas());
    }

    private void Start()
    {
        numberOfSpawners = FindObjectsOfType<Spawner>().Length;
        maxnumberOfSpawners = numberOfSpawners;
        SetSpawnersText(numberOfSpawners, maxnumberOfSpawners);
        SetKilledZombiesText(numberOfKilledZombies);
        SetAliveZombiesText(numberOfAliveZombies);

        undergroundCanvas.enabled = false;
        wonGameCanvas.enabled = false;
        ranOutOfTimeCanvas.enabled = false;
        timerText.enabled = false;    
        spawnersText.enabled = false;
        aliveZombiesText.enabled = false;
        killedZombiesText.enabled = false;

        startingTimeRemainingInSeconds = remainingTimeInSeconds;
    }

    private void Update()
    {
        if (wasCanvasDisplayed)
        {
            remainingTimeInSeconds -= Time.deltaTime;
            timeSpan = TimeSpan.FromSeconds(remainingTimeInSeconds);
            timerText.text = $"Time Left: {timeSpan.Minutes}:{timeSpan.Seconds}";

            if (remainingTimeInSeconds <= 0 && !isGameFinished)
            {
                isGameFinished = true;
                DisplayRanOutOfTimeCanvas();
                Debug.Log("You lose!");
            }
        }
    }

    public void SetSpawnersText(int numberOfSpawners, int maxNumberOfSpawners)
    {
        spawnersText.text = $"Spawners: {numberOfSpawners}/{maxNumberOfSpawners}";
    }

    public void SetKilledZombiesText(int numberOfKilledZombies)
    {
        killedZombiesText.text = $"Zombies Killed: {numberOfKilledZombies}";
    }

    public void SetAliveZombiesText(int numberOfAliveZombies)
    {
        aliveZombiesText.text = $"Zombies Alive: {numberOfAliveZombies}";
    }

    // Canvas that is displayed when the player enters the underground
    public void DisplayUndergroundCanvas()
    {
        if (!wasCanvasDisplayed)
        {
            // Pause game and show cursor
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;        
            Cursor.visible = true;
            FindObjectOfType<WeaponSwitcher>().enabled = false;
            // FindObjectOfType<WeaponZoom>().enabled = false;
            undergroundPromptText.text = $"Try: {PlayerPerformanceManager.Instance.NumberOfSpeedrunTry + 1}";
            undergroundCanvas.enabled = true;
            guiCanvas.enabled = false;

        }
    }
    
    public void HideUndergroundCanvas()
    {
        guiCanvas.enabled = true;
        undergroundCanvas.enabled = false;
        wasCanvasDisplayed = true;
        timerText.enabled = true;    
        spawnersText.enabled = true;
        aliveZombiesText.enabled = true;
        killedZombiesText.enabled = true;
       

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;        
        Cursor.visible = false;
        FindObjectOfType<WeaponSwitcher>().enabled = true;
        // FindObjectOfType<WeaponZoom>().enabled = true;
    }

    public void DisplayWonGameCanvas()
    {
        guiCanvas.enabled = false;
        wonGameCanvas.enabled = true;
        winnerCanvasTimeLeftText.text = $"Time Left: {timeSpan.Minutes}:{timeSpan.Seconds}";

        // Pause game and show cursor
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        WeaponZoom weaponZoom = FindObjectOfType<WeaponZoom>();
        if (weaponZoom)
        {
            weaponZoom.enabled = false;
        }  
        PassStatsToPlayerPerformanceManager(true);
    }

    public void DisplayRanOutOfTimeCanvas()
    {
        guiCanvas.enabled = false;
        ranOutOfTimeCanvas.enabled = true;
        // Pause game and show cursor
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;        
        Cursor.visible = true;
        FindObjectOfType<WeaponSwitcher>().enabled = false;

        WeaponZoom weaponZoom = FindObjectOfType<WeaponZoom>();
        if (weaponZoom)
        {
            weaponZoom.enabled = false;
        }  

        PassStatsToPlayerPerformanceManager(false);
    }

    public void PassStatsToPlayerPerformanceManager(bool finished)
    {
        // First 2 tries are not getting recorded
        if (PlayerPerformanceManager.Instance.NumberOfSpeedrunTry > 1)
        {
            float timeOfCompletionInSeconds = startingTimeRemainingInSeconds - remainingTimeInSeconds;
            PlayerPerformanceManager.Instance.UpdateManagedToFinishRound(finished);
            PlayerPerformanceManager.Instance.UpdateTimeOfCompletion(timeOfCompletionInSeconds);
            PlayerPerformanceManager.Instance.DisplayPlayerStats();
            PlayerPerformanceManager.Instance.NumberOfSpeedrunTry += 1;
        }
        else
        {
            PlayerPerformanceManager.Instance.ResetPlayerStats(false);
            PlayerPerformanceManager.Instance.NumberOfSpeedrunTry += 1;
        }
    }

    
}
