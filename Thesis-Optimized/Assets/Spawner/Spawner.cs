using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] [Range(0, 20)] private int zombiesToSpawn = 5;
    [SerializeField] [Range(0.1f, 10.0f)] private float spawnRate = 5.0f;
    [SerializeField] private float maxSpawnedZombieSpeed = 2.0f;
    [SerializeField] private float minSpawnedZombieSpeed = 2.0f;
    [SerializeField] private float spawnedZombieHP = 100.0f;
    [SerializeField] private AudioClip destroySound;
    [SerializeField] private ObjectPool objectPool;
    private EnemyHealth enemyHealth;
    private EnemyAISpawned enemyAISpawned;
    private NavMeshAgent navMeshAgent;
    private int zombiesToSpawnCounter;
    private int spawnedZombies = 0;
    private MeshRenderer meshRenderer;
    private SpawnerManager spawnerManager;
    private MeshCollider meshCollider;
    Dictionary<EnemyHealth, GameObject> spawnedZombiesDictionary = new Dictionary<EnemyHealth, GameObject>();

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();    
        spawnerManager = GetComponentInParent<SpawnerManager>();
        meshCollider = GetComponent<MeshCollider>();
    }

    private void Start()
    {
        spawnerManager.AddSpawner();
    }

    public void ActivateSpawner()
    {
        zombiesToSpawnCounter = zombiesToSpawn;
        StartCoroutine(SpawnZombies());
    }

    private IEnumerator SpawnZombies()
    {
        while (zombiesToSpawn > spawnedZombies)
        {
            // GameObject zombieGameObject = Instantiate(zombiePrefab, transform);
            GameObject zombieGameObject = objectPool.SpawnObject(transform.position, transform.rotation);
            if (zombieGameObject != null)
            {
                enemyAISpawned = zombieGameObject.GetComponent<EnemyAISpawned>();
                if (enemyAISpawned != null)
                {
                    enemyAISpawned.UpdateParentSpawner(this);
                }

                enemyHealth = zombieGameObject.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    // Set properties for the spawned zombie
                    enemyHealth.SetHP(spawnedZombieHP);
                    navMeshAgent = enemyHealth.GetComponent<NavMeshAgent>();
                    if (navMeshAgent != null)
                    {
                        navMeshAgent.speed = UnityEngine.Random.Range(minSpawnedZombieSpeed, maxSpawnedZombieSpeed + 1);
                        Debug.Log("Zombie speed: " + navMeshAgent.speed);
                    }

                    // Add the spawned zombie to the dictionary and update the number of alive zombies
                    spawnedZombiesDictionary.Add(enemyHealth, zombieGameObject);
                    spawnerManager.AddAliveZombie();
                    spawnedZombies++;
                }
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    public void RemoveZombie(EnemyHealth zombieToRemove)
    {
        // Remove from the dictionary and update the number of alive & killed zombies
        if (spawnedZombiesDictionary.ContainsKey(zombieToRemove))
        {
            StartCoroutine(DestroyZombie(zombieToRemove));
        }
    }

    private IEnumerator DestroyZombie(EnemyHealth zombieToRemove)
    {
        GameObject zombieGameObject = spawnedZombiesDictionary[zombieToRemove];
        spawnedZombiesDictionary.Remove(zombieToRemove);
        zombiesToSpawnCounter--;
        spawnerManager.RemoveAliveZombie();
        spawnerManager.AddKilledZombie();

        if (zombiesToSpawnCounter <= 0)
        {
            // Destroy(gameObject);
            meshRenderer.enabled = false;
            meshCollider.enabled = false;
            spawnerManager.RemoveSpawner();
            SoundManager.Instance.Play(destroySound);
        }


        yield return new WaitForSeconds(2.0f);
        objectPool.ReturnObject(zombieGameObject);
    }
}
