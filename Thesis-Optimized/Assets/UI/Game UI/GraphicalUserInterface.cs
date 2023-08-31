using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphicalUserInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI ammoText;

    public void UpdateHealthText(float currentHealth)
    {
        healthText.text = $"HP: {currentHealth}";
    }

    public void UpdateAmmoText(int ammoCount, AmmoType ammoType)
    {
        ammoText.text = $"{ammoType}: {ammoCount}";
    }
}
