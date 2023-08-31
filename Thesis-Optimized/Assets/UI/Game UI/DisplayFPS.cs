using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayFPS : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    private float deltaTime;
    private float fps;
 
    void Update ()
    {
        if (Time.timeScale != 0)
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            fps = 1.0f / deltaTime;
            fpsText.text = $"FPS: {Mathf.Ceil (fps)}";
        }
     }
}
