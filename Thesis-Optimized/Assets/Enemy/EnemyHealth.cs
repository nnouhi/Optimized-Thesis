using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private AudioClip[] zombieDeathSounds;
    
    private float currentHealth;
    private bool isDead = false;
    public bool IsDead { get { return isDead; } }
    private AudioSource audioSource;
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;
    private EnemyAISpawned enemyAISpawned;
    private Animator animator;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        sphereCollider = GetComponentInChildren<SphereCollider>(); 
        enemyAISpawned = GetComponent<EnemyAISpawned>();
        animator = GetComponent<Animator>();
        SetHP(maxHealth);        
    }

    public void SetHP(float amount)
    {
        currentHealth = amount;
    }

    public void TakeDamage(float amount)
    {
        BroadcastMessage("OnDamageTaken");
        currentHealth -= amount;
        Debug.Log("Zombie HP: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            if (animator != null)
            {
                animator.SetTrigger("Die");
            }
                

            int index = UnityEngine.Random.Range(0, zombieDeathSounds.Length);
            SoundManager.Instance.PlayAtLocation(zombieDeathSounds[index], transform.position);

            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }

            if (sphereCollider != null)
            {
                sphereCollider.enabled = false;
            }

            if (enemyAISpawned != null)
            {
                Spawner spawner = enemyAISpawned.GetParentSpawner();
                if (spawner != null)
                {
                    spawner.RemoveZombie(this);
                }  
            }     
        }
    }

    private void OnDisable()
    {
        // This game was 3 type of zombies, enemyAI, enemyAISpawned, and enemyAIChase that share the enemy health script
        // Only the enemyAISpawned which is getting pooled gets disabled, therefor the OnDisable is getting called only on the enemyAISpawned
        // Thats why we don't need to check if the enemyAISpawned is null

        // Reset some of the components/attributres for the AI to work once it gets enabled from the pool
        if (enemyAISpawned == null)
        {
            return;
        }
        
        if (isDead)
        {
            // reset components
            if (boxCollider != null)
            {
                boxCollider.enabled = true;
            }

            if (sphereCollider != null)
            {
                sphereCollider.enabled = true;
            }

            SetHP(100.0f);
            isDead = false;

            // Reset enemyAISpawned components and attributes
            enemyAISpawned.reset();
        }
    }
}
