using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIPatrol : MonoBehaviour
{
    [SerializeField] private float chaseRange = 20.0f;
    [SerializeField] private float patrolRange = 10.0f;
    [SerializeField] private AudioClip[] growlSounds;
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private EnemyHealth health;
    private float distanceToPlayer = Mathf.Infinity;
    private float distanceToPatrolPoint = Mathf.Infinity;
    private bool isProvoked = false;
    private float turnSpeed = 5.0f;
    private Vector3 directionToTarget = Vector3.zero;
    private Vector3 patrolPoint = Vector3.zero;
    private float angle = 0.0f;
    private bool isInFront = false;
    private AudioSource audioSource;
    private bool isGrowling = false;

    private void Awake() 
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
        GetNewPatrolPoint();
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
        else
        {
            // If the player is in range, start chasing
            if (distanceToPlayer <= chaseRange && isInFront)
            {
                isProvoked = true;
            }
            // If the player is out of range and not provoked, patrol
            else
            {
                FaceTarget(patrolPoint);
                Patrol();
            }
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

        FaceTarget(playerTransform.position);

        if (distanceToPlayer >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }
        else
        {
            AttackTarget();
        }  
    }

    private void Patrol()
    {
        animator.SetBool("Chase", false);
        animator.SetBool("Patrol", true);
        navMeshAgent.SetDestination(patrolPoint);
        distanceToPatrolPoint = Vector3.Distance(transform.position, patrolPoint);
        if (distanceToPatrolPoint <= navMeshAgent.stoppingDistance)
        {
            GetNewPatrolPoint();
        }
    }

    private void ChaseTarget()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Chase", true);
        animator.SetBool("Patrol", false);
        navMeshAgent.SetDestination(playerTransform.position);
    }
    
    private void AttackTarget()
    {
        animator.SetBool("Attack", true);
    }

    private void FaceTarget(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0.0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void DetermineIfChase()
    {
        directionToTarget = (playerTransform.position - transform.position).normalized;
        angle = Vector3.Angle(directionToTarget, transform.forward);
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        isInFront = MathF.Abs(angle) <= 90.0f;
    }

    private void GetNewPatrolPoint()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRange;
        randomDirection += transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomDirection, out navMeshHit, patrolRange, 1);
        patrolPoint = navMeshHit.position;
    }

    // Visualize provoked range
    private void OnDrawGizmosSelected()
    {
        // Display the chase radius when selected
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
