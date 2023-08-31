using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    [SerializeField] private GameObject[] nearbySpawners;
    private bool wasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !wasTriggered)
        {
            wasTriggered = true;
            foreach (GameObject spawner in nearbySpawners)
            {
                spawner.GetComponent<Spawner>().ActivateSpawner();
            }
        }
    }
}
