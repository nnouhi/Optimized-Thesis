using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private float zoomedInFOV = 30.0f;
    [SerializeField] RigidbodyFirstPersonController fpsController;
    private float zoomedOutFOV;
    private float sensitivityX;
    private float sensitivityY;
    private float zoomedInSensitivityX = 0.5f;
    private float zoomedInSensitivityY = 0.5f;

    private void OnDisable()
    {
        UpdateCameraFOV(zoomedOutFOV);
        UpdateSensitivity(sensitivityX, sensitivityY);
    }

    private void Start()
    {
        zoomedOutFOV = fpsCamera.fieldOfView;
        sensitivityX = fpsController.mouseLook.XSensitivity;
        sensitivityY = fpsController.mouseLook.YSensitivity;
    }

    private void Update()
    {
        // Zoom in
        if (Input.GetMouseButtonDown(1))
        {
            UpdateCameraFOV(zoomedInFOV);
            UpdateSensitivity(sensitivityX * 0.5f, sensitivityY * 0.5f);
        }

        // Zoom out
        if (Input.GetMouseButtonUp(1))
        {
            UpdateCameraFOV(zoomedOutFOV);
            UpdateSensitivity(sensitivityX, sensitivityY);
        }    
    }

    private void UpdateCameraFOV(float newFOV)
    {
        fpsCamera.fieldOfView = newFOV;
    }

    private void UpdateSensitivity(float newMouseSensitivityX, float newMouseSensitivityY)
    {
        
        fpsController.mouseLook.XSensitivity = newMouseSensitivityX;
        fpsController.mouseLook.YSensitivity = newMouseSensitivityY;
    }
}
