using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class PlayerPerformanceExporter : MonoBehaviour
{
    private PlayerPerformanceManager ppm;

    private void Awake()
    {
        ppm = PlayerPerformanceManager.Instance;
    }

    public void ExportPlayerStatsToCSV(float damageTaken, float timeOfCompletionInSeconds, float shotsFired,
     float shotsHit, float headShotsHit,
    float accuracy, float headShotAccuracy, int maxHeadShotStreak,
    float averageHeadShotDistance, float averageRotationRate, float overallDistanceTraveled, bool finishedRoundSuccessfully)
    {
        string filePath;
        if (GameManager.Instance.TargetFrameRate == 60)
        {
            filePath = Application.persistentDataPath + "/PlayerStats60FPS.csv";
        }
        else if (GameManager.Instance.TargetFrameRate == 30)
        {
            filePath = Application.persistentDataPath + "/PlayerStats30FPS.csv";
        }
        else
        {
            filePath = Application.persistentDataPath + "/PlayerStats15FPS.csv";
        }
        
        Debug.Log(filePath);
        bool fileExists = File.Exists(filePath);
        // Create the CSV file and write the header row.
        using (StreamWriter sw = new StreamWriter(filePath, fileExists))
        {
            string header = "Damage Taken,Completion Time (minutes),Completion Time (seconds),Shots Fired,Shots Hit,Accuracy (%),Head Shot Accuracy (%),Max Head Shot Streak,Average Head Shot Distance,Average Rotation Rate (deg),Overall Distance Traveled,Finished Round Successfully";
            if (!fileExists)
            {
                sw.WriteLine(header);    
            }

            // Write the data row.
        TimeSpan ts = TimeSpan.FromSeconds(timeOfCompletionInSeconds);
            sw.WriteLine($"{damageTaken},{ts.Minutes},{ts.Seconds},{shotsFired},{shotsHit},{accuracy},{headShotAccuracy},{maxHeadShotStreak},{averageHeadShotDistance},{averageRotationRate},{overallDistanceTraveled},{finishedRoundSuccessfully}");
        }
    }

    public void DeleteCSVFiles()
    {
        string filePath60FPS = Application.persistentDataPath + "/PlayerStats60FPS.csv";
        string filePath30FPS = Application.persistentDataPath + "/PlayerStats30FPS.csv";
        string filePath15FPS = Application.persistentDataPath + "/PlayerStats15FPS.csv";

        if (File.Exists(filePath60FPS))
        {
            File.Delete(filePath60FPS);
        }
        if (File.Exists(filePath15FPS))
        {
            File.Delete(filePath15FPS);
        }
        if (File.Exists(filePath30FPS))
        {
            File.Delete(filePath30FPS);
        }
    }
}