using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int ammoAmount = 10;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Ammo>().IncreaseCurrentAmmo(ammoType, ammoAmount);
            SoundManager.Instance.Play(pickupSound);
            gameObject.SetActive(false);
            // Destroy(gameObject);
        }
    }
}
