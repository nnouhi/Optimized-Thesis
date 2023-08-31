using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField] private float lightDecay = 0.1f;
    [SerializeField] private float angleDecay = 0.1f;
    [SerializeField] private float minAngle = 40.0f;
    Light myLight;

    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        DecreaseLightAngle();
        DecreaseLightIntensity();
    }

    private void DecreaseLightAngle()
    {
        if (myLight.spotAngle >= minAngle)
        {
            myLight.spotAngle -= angleDecay * Time.deltaTime;
        }
    }

    private void DecreaseLightIntensity()
    {
        if (myLight.intensity >= 0.0f)
        {
            myLight.intensity -= lightDecay * Time.deltaTime;
        }
    }

    public void RestoreLightAngle(float angle)
    {
        myLight.spotAngle = angle;
    }

    public void RestoreLightIntensity(float intensity)
    {
        myLight.intensity += intensity;
    }
}
