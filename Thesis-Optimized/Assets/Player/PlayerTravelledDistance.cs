using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTravelledDistance : MonoBehaviour
{
    private Vector3 lastPosition;
    private float totalDistance;
    
    private void Start()
    {
        lastPosition = transform.position;
    }
    
    private void Update()
    {
        // If game is not paused
        if (Time.timeScale != 0)
        {
            float distance = Vector3.Distance(lastPosition, transform.position);
            totalDistance += distance;
            lastPosition = transform.position;
            PlayerPerformanceManager.Instance.UpdateOverallDistanceTraveled(totalDistance);
        }
    }
    
}
