using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UserDemographic : MonoBehaviour
{
    [SerializeField] private Button submitButton;
    [SerializeField] private Color defaultButtonColor;
    [SerializeField] private Color selectedButtonColor;
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas userDemographicCanvas;

    [Header("Amount of hours played per week")]
    [SerializeField] private Button zeroHoursButton; 
    [SerializeField] private Button oneToTwoHoursButton; 
    [SerializeField] private Button threeToFiveHoursButton; 
    [SerializeField] private Button sixToTenHoursButton; 
    [SerializeField] private Button elevenPlusHoursButton;

    [Header("Experience in computer games")]
    [SerializeField] private Button oneExperienceButton; 
    [SerializeField] private Button twoExperienceButton; 
    [SerializeField] private Button threeExperienceButton; 
    [SerializeField] private Button fourExperienceButton; 
    [SerializeField] private Button fiveExperienceButton;  

    [Header("Experience in FPS games")]
    [SerializeField] private Button oneFPSExperienceButton; 
    [SerializeField] private Button twoFPSExperienceButton; 
    [SerializeField] private Button threeFPSExperienceButton; 
    [SerializeField] private Button fourFPSExperienceButton; 
    [SerializeField] private Button fiveFPSExperienceButton;  

    private string amountOfHoursPlayedPerWeek = "";
    private int experienceInComputerGames = 0;
    private int experienceInFPSGames = 0;

    private void Awake() 
    {
        // Add listeners to buttons
        zeroHoursButton.onClick.AddListener(() => SetAmountOfHoursPlayedPerWeek("0 hours", zeroHoursButton));
        oneToTwoHoursButton.onClick.AddListener(() => SetAmountOfHoursPlayedPerWeek("1 - 2 hours", oneToTwoHoursButton));
        threeToFiveHoursButton.onClick.AddListener(() => SetAmountOfHoursPlayedPerWeek("3 - 5 hours", threeToFiveHoursButton));
        sixToTenHoursButton.onClick.AddListener(() => SetAmountOfHoursPlayedPerWeek("6 - 10 hours", sixToTenHoursButton));
        elevenPlusHoursButton.onClick.AddListener(() => SetAmountOfHoursPlayedPerWeek("11+ hours", elevenPlusHoursButton));

        oneExperienceButton.onClick.AddListener(() => SetExperienceInComputerGames(1, oneExperienceButton));
        twoExperienceButton.onClick.AddListener(() => SetExperienceInComputerGames(2, twoExperienceButton));
        threeExperienceButton.onClick.AddListener(() => SetExperienceInComputerGames(3, threeExperienceButton));
        fourExperienceButton.onClick.AddListener(() => SetExperienceInComputerGames(4, fourExperienceButton));
        fiveExperienceButton.onClick.AddListener(() => SetExperienceInComputerGames(5, fiveExperienceButton));

        oneFPSExperienceButton.onClick.AddListener(() => SetExperienceInFPSGames(1, oneFPSExperienceButton));
        twoFPSExperienceButton.onClick.AddListener(() => SetExperienceInFPSGames(2, twoFPSExperienceButton));
        threeFPSExperienceButton.onClick.AddListener(() => SetExperienceInFPSGames(3, threeFPSExperienceButton));
        fourFPSExperienceButton.onClick.AddListener(() => SetExperienceInFPSGames(4, fourFPSExperienceButton));
        fiveFPSExperienceButton.onClick.AddListener(() => SetExperienceInFPSGames(5, fiveFPSExperienceButton));

        submitButton.onClick.AddListener(() => OnSubmit());

        PlayerPrefs.DeleteKey("UserDemographic");
        if (PlayerPrefs.HasKey("UserDemographic"))
        {
            Debug.Log("User demographic already submitted");
            mainMenuCanvas.gameObject.SetActive(true);
            userDemographicCanvas.gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("UserDemographic", 1);
        }

    }

    private void SetAmountOfHoursPlayedPerWeek(string hours, Button pressedButton)
    {
        amountOfHoursPlayedPerWeek = hours;

        // set the other buttons to default color
        zeroHoursButton.image.color = defaultButtonColor;
        oneToTwoHoursButton.image.color = defaultButtonColor;
        threeToFiveHoursButton.image.color = defaultButtonColor;
        sixToTenHoursButton.image.color = defaultButtonColor;
        elevenPlusHoursButton.image.color = defaultButtonColor;
     
        // set the button color to selected
        pressedButton.image.color = selectedButtonColor;
    }

    private void SetExperienceInComputerGames(int experience, Button pressedButton)
    {
        experienceInComputerGames = experience;

        // set the other buttons to default color
        oneExperienceButton.image.color = defaultButtonColor;
        twoExperienceButton.image.color = defaultButtonColor;
        threeExperienceButton.image.color = defaultButtonColor;
        fourExperienceButton.image.color = defaultButtonColor;
        fiveExperienceButton.image.color = defaultButtonColor;
     
        // set the button color to selected
        pressedButton.image.color = selectedButtonColor;
    }

    private void SetExperienceInFPSGames(int experience, Button pressedButton)
    {
        experienceInFPSGames = experience;

        // set the other buttons to default color
        oneFPSExperienceButton.image.color = defaultButtonColor;
        twoFPSExperienceButton.image.color = defaultButtonColor;
        threeFPSExperienceButton.image.color = defaultButtonColor;
        fourFPSExperienceButton.image.color = defaultButtonColor;
        fiveFPSExperienceButton.image.color = defaultButtonColor;
     
        // set the button color to selected
        pressedButton.image.color = selectedButtonColor;
    }

    private void OnSubmit()
    {
        // Validate the data
        if (amountOfHoursPlayedPerWeek == "" || experienceInComputerGames == 0 || experienceInFPSGames == 0)
        {
            Debug.Log("Invalid data");
            return;
        }

        // Call 
        string filePath;
        filePath = Application.persistentDataPath + "/UserDemographic.csv";
    
        bool fileExists = File.Exists(filePath);
        
        // Create the CSV file and write the header row.
        using (StreamWriter sw = new StreamWriter(filePath, fileExists))
        {
            if (!fileExists)
            {
                sw.WriteLine("Hours Played Per Week,Experience In Computer Games ((1)Casual - (5)Hardcore),Experience In FPS Games ((1)Worst - (5)Best)");    
            }

            // Write the data row.
            sw.WriteLine($"{amountOfHoursPlayedPerWeek},{experienceInComputerGames},{experienceInFPSGames}");

            userDemographicCanvas.gameObject.SetActive(false);
            mainMenuCanvas.gameObject.SetActive(true);
        }
    }
}
