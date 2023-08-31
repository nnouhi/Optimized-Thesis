using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private float range = 100.0f;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Ammo ammoSlot;
    [SerializeField] private float timeBeforeShots = 0.5f;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private GraphicalUserInterface gui;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip headShotSound;
    [SerializeField] private ObjectPool objectPool;


    private RaycastHit hit;
    private EnemyHealth targetHealthcomponent;
    private System.Diagnostics.Stopwatch stopWatch;
    private TimeSpan timeSpan;
    private bool isReloading = false;
    private float timeLeft;
    private bool canShoot = true;
    private int headshotMultiplier = 2;
    private float hitVFXDestroyTime = 0.1f;
    private bool isHitImpactSpawned = false;
    private  GameObject impact;


    private void OnEnable() 
    {
        // If the weapon is reloading, wait for the time left before shooting again
        if (isReloading)
        {
            StartCoroutine(ResetCanShoot());
        } 
    }

    private void OnDisable() 
    {
        // If the weapon is reloading, stop the timer and calculate the time left
        if (stopWatch.IsRunning)
        {
            timeLeft = 0.0f;
            stopWatch.Stop();
            timeSpan = stopWatch.Elapsed;
            timeLeft = timeBeforeShots - (float)timeSpan.TotalSeconds;
            isReloading = true;
        }
        
        // There was a bug if you shot and then switched weapons, the hit effect would stay on the screen
        // This is a fix for that
        if (isHitImpactSpawned)
        {
            if (impact != null)
            {
                objectPool.ReturnObject(impact);
            }
            isHitImpactSpawned = false;
        }
    }

    private void Start()
    {
        stopWatch = new System.Diagnostics.Stopwatch();
    }

    private void Update()
    {
        gui.UpdateAmmoText(ammoSlot.GetAmmoCount(ammoType), ammoType);
        if (Input.GetMouseButton(0) & canShoot && Time.timeScale != 0)
        {
           StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        int currentAmmo = ammoSlot.GetAmmoCount(ammoType);
        stopWatch.Start();
        canShoot = false;
        if (currentAmmo > 0)
        {
            ammoSlot.ReduceCurrentAmmo(ammoType);
            PlayMuzzleFlash();
            ProcessRayCast();
            
            SoundManager.Instance.Play(shootSound);
        }
        else
        {
            SoundManager.Instance.Play(clickSound);
        }

        yield return new WaitForSeconds(timeBeforeShots);

        stopWatch.Stop();
        canShoot = true;
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play(); 
    }

    private void ProcessRayCast()
    {
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            StartCoroutine(CreateHitImpact(hit));
            targetHealthcomponent = hit.transform.GetComponentInParent<EnemyHealth>();
            if (targetHealthcomponent != null)
            {
                if (hit.transform.tag == "Head")
                {
                    SoundManager.Instance.PlayAtLocation(headShotSound, hit.point);
                    targetHealthcomponent.TakeDamage(damage * headshotMultiplier);
                }
                else if (hit.transform.tag == "Enemy")
                {
                    targetHealthcomponent.TakeDamage(damage);
                }
            }
        }
        else
        {
            return;
        }
    }

    private IEnumerator CreateHitImpact(RaycastHit hit)
    {
        impact = objectPool.SpawnObject(hit.point, Quaternion.LookRotation(hit.normal));
        isHitImpactSpawned = true;

        // Return object to pool after x seconds
        yield return new WaitForSeconds(hitVFXDestroyTime);
        objectPool.ReturnObject(impact);
        isHitImpactSpawned = false;
    }

    private IEnumerator ResetCanShoot()
    {
        yield return new WaitForSeconds(timeLeft);
        canShoot = true;
        isReloading = false;
    }
}
