using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{
    [SerializeField] private Canvas gamePauseCanvas;
    [SerializeField] private Canvas guiCanvas;

    private bool onSpeedrunMap = false;

    private void Start()
    {
        gamePauseCanvas.enabled = false;
        onSpeedrunMap = SceneManager.GetActiveScene().buildIndex == 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
           if (Time.timeScale != 0)
            {
                if (gamePauseCanvas.enabled)
                {
                    return;
                }
                else
                {
                    DisplayPause();
                }
            }
        }

        // Restart level if we are on speedrun map
        if (Input.GetKeyDown(KeyCode.R) && onSpeedrunMap)
        {
            if (gamePauseCanvas.enabled)
            {
                FindObjectOfType<MenuButtons>().RestartGame();
            }
        }

        // Restart number of tries if we are on speedrun map
        if (Input.GetKeyDown(KeyCode.T) && onSpeedrunMap)
        {
            if (gamePauseCanvas.enabled)
            {
                FindObjectOfType<MenuButtons>().ResetNumberOfTries();
            }
        }
    }

    public void DisplayPause()
    {
        gamePauseCanvas.enabled = true;
        guiCanvas.enabled = false;

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
    }
}
