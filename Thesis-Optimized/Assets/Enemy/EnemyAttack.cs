using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float maxDamage = 20.0f;
    [SerializeField] private float minDamage = 10.0f;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float damage;
    private PlayerHealth target;
    private DisplayDamage displayDamage;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.GetComponent<PlayerHealth>();
            displayDamage = player.GetComponent<DisplayDamage>();
        }
        
        // target = FindObjectOfType<PlayerHealth>();
        damage = Mathf.Floor(UnityEngine.Random.Range(minDamage, maxDamage + 1));
    }

    // Invoked from the animation event
    public void AttackHitEvent()
    {
        if (target != null)
        {
            target.TakeDamage(damage);
            if (displayDamage != null)
            {
                displayDamage.OnDamageTaken();
            }

           SoundManager.Instance.Play(attackSound);
        }
    }
}
