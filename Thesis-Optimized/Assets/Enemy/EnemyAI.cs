using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float chaseRange = 20.0f;
    [SerializeField] private AudioClip[] growlSounds;

    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private EnemyHealth health;
    private float distanceToPlayer = Mathf.Infinity;
    private bool isProvoked = false;
    private float turnSpeed = 5.0f;
    private Vector3 directionToPlayer = Vector3.zero;
    private float angle = 0.0f;
    private bool isInFront = false;
    private AudioSource audioSource;
    private bool isGrowling = false;

    void Awake() 
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        audioSource = GetComponent<AudioSource>(); 
    }
    void Start()
    {
        // playerTransform = GameObject.FindWithTag("Player").transform;
        // animator = GetComponent<Animator>();
        // navMeshAgent = GetComponent<NavMeshAgent>();
        // health = GetComponent<EnemyHealth>();
        // audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (health.IsDead)
        {
            enabled = false;
            navMeshAgent.enabled = false;
            return;
        }

        DetermineIfChase();

        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToPlayer <= chaseRange && isInFront)
        {
            isProvoked = true;
        }
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    private void EngageTarget()
    {
        if (!isGrowling)
        {
            isGrowling = true;
            if (audioSource != null)
            {
                int index = UnityEngine.Random.Range(0, growlSounds.Length);
                // audioSource.PlayOneShot(growlSounds[index]);
                // AudioSource.PlayClipAtPoint(growlSounds[index], transform.position);
                SoundManager.Instance.PlayAtLocation(growlSounds[index], transform.position);
            }
        }
        FacePlayer();

        if (distanceToPlayer >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }
        else
        {
            AttackTarget();
        }  
    }

    private void ChaseTarget()
    {
        animator.SetBool("Attack", false);
        animator.SetTrigger("Move");
        navMeshAgent.SetDestination(playerTransform.position);
    }
    
    private void AttackTarget()
    {
        animator.SetBool("Attack", true);
    }

    private void FacePlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0.0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void DetermineIfChase()
    {
        directionToPlayer = (playerTransform.position - transform.position).normalized;
        angle = Vector3.Angle(directionToPlayer, transform.forward);
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        isInFront = MathF.Abs(angle) <= 90.0f;
    }

    // Visualize provoked range
    private void OnDrawGizmosSelected()
    {
        // Display the chase radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
