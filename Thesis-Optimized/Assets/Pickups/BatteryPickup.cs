using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    [SerializeField] private float restoreAngle = 70.0f;
    [SerializeField] private float addIntensity = 1.0f;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInChildren<FlashLight>().RestoreLightAngle(restoreAngle);
            other.gameObject.GetComponentInChildren<FlashLight>().RestoreLightIntensity(addIntensity);
            SoundManager.Instance.Play(pickupSound);
            gameObject.SetActive(false);
            // Destroy(gameObject);
        }
    }
}
