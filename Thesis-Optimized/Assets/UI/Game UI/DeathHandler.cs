using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathHandler : MonoBehaviour
{
   [SerializeField] private Canvas youDiedCanvas;
   [SerializeField] private Canvas guiCanvas;
    private UndergroundBehavior undergroundBehavior;
    private bool isInSpeedrunMap = false;

    private void Start()
    {
        youDiedCanvas.enabled = false;
        isInSpeedrunMap = SceneManager.GetActiveScene().buildIndex == 2;

        if (isInSpeedrunMap)
        {
            undergroundBehavior = FindObjectOfType<UndergroundBehavior>();
        }
    }

    public void HandleDeath()
    {
        youDiedCanvas.enabled = true;
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

        if (isInSpeedrunMap)
        {
            undergroundBehavior.PassStatsToPlayerPerformanceManager(false);
        }
    }
}
