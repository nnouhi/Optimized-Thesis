using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationRate : MonoBehaviour
{
    public float totalRotation;
    public float totalTime;

    private Quaternion previousRotation;

    private void Start()
    {
        previousRotation = transform.rotation;
    }

    private void Update()
    {
        // If not paused
        if (Time.timeScale != 0)
        {
            float deltaRotation = Quaternion.Angle(transform.rotation, previousRotation);
            totalRotation += deltaRotation;
            totalTime += Time.deltaTime;
            previousRotation = transform.rotation;
            PlayerPerformanceManager.Instance.UpdateTotalRotation(totalRotation);
            PlayerPerformanceManager.Instance.UpdateTotalTime(totalTime);
        }
    }
}
