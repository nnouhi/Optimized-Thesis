using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private GraphicalUserInterface gui;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        gui.UpdateHealthText(currentHealth);
        // Application.targetFrameRate = 15;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        gui.UpdateHealthText(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<DeathHandler>().HandleDeath();
    }
}
