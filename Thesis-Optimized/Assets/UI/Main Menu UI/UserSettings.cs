using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserSettings : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI musicVolumeSliderText;
    [SerializeField] private TextMeshProUGUI sensitivitySliderText;
    [SerializeField] private TextMeshProUGUI fpsText;

    [Header("Sliders")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sensitivitySlider;

    [Header("Buttons")]
    [SerializeField] private Button FPSButton15;
    [SerializeField] private Button FPSButton30;
    [SerializeField] private Button FPSButton60;

    private void Awake()
    {
        musicVolumeSlider.onValueChanged.AddListener((newValue) => MusicVolumeSliderValueChanged(newValue));
        sensitivitySlider.onValueChanged.AddListener((newValue) => SensitivitySliderValueChanged(newValue));
        FPSButton15.onClick.AddListener(() => FPSButtonClicked(15));
        FPSButton30.onClick.AddListener(() => FPSButtonClicked(30));
        FPSButton60.onClick.AddListener(() => FPSButtonClicked(60));

        // Get values from game manager
        musicVolumeSlider.value = GameManager.Instance.MusicVolume;
        sensitivitySlider.value = GameManager.Instance.MouseSensitivity;
        fpsText.text = GameManager.Instance.TargetFrameRate.ToString();
    }

    private void FPSButtonClicked(int fps)
    {
        GameManager.Instance.TargetFrameRate = fps;
        fpsText.text = fps.ToString();
    }

    private void MusicVolumeSliderValueChanged(float newValue)
    {
        musicVolumeSliderText.text = newValue.ToString();
        musicVolumeSlider.value = newValue;
        GameManager.Instance.MusicVolume = newValue;
    }

    private void SensitivitySliderValueChanged(float newValue)
    {
        sensitivitySliderText.text = newValue.ToString();
        sensitivitySlider.value = newValue;
        PlayerPrefs.SetFloat("MouseSensitivity", newValue);
        GameManager.Instance.MouseSensitivity = newValue;
    }
}
