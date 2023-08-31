using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPerformanceManager : MonoBehaviour
{
	// Singleton instance.
	public static PlayerPerformanceManager Instance = null;
    [SerializeField] private PlayerPerformanceExporter playerPerformanceExporter;

    [Header("Player Performance")]
    private float overallDistanceTraveled = 0.0f;
    private float totalRotation = 0;
    private float totalTime = 0.0f;
    private float averageRotationRate = 0;
    private bool finishedRoundSuccessfully = false;
    private float damageTaken = 0.0f;
    private float timeOfCompletionInSeconds = 0.0f;
    private float shotsFired = 0.0f;
    private float shotsHit = 0.0f;
    private float headShotsHit = 0.0f;
    private float accuracy = 0.0f;
    private float headShotAccuracy = 0.0f;
    private int maxHeadShotStreak = 0;
    private bool isInHeadshotStreak = false;
    private int headShotStreakCount = 0;
    private float totalHeadShotDistance = 0;
    private float totalClosestEnemyDistance = 0;
    private float averageHeadShotDistance = 0;
    private float averageClosestEnemyDistance = 0;
    private List<int> frameRates; 
    private int numberOfSpeedrunTry = 0;
    public int NumberOfSpeedrunTry 
    {
        get
        {
            if (numberOfSpeedrunTry >= frameRates.Count)
            {
                numberOfSpeedrunTry = 0;
            }
            GameManager.Instance.TargetFrameRate = frameRates[numberOfSpeedrunTry];
            return numberOfSpeedrunTry;
        }
        
        set => numberOfSpeedrunTry = value;
    }
    

	// Initialize the singleton instance.
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
            frameRates = new List<int>{60, 60, 60, 30, 30, 30, 15, 15, 15};
            frameRates = frameRates.OrderBy(x => Guid.NewGuid()).ToList();

            // Add 2 random frame rates for the first 2 rounds that don't count
            int randomIndex = UnityEngine.Random.RandomRange(0, frameRates.Count);
            frameRates.Insert(0, frameRates[randomIndex]);
            randomIndex = UnityEngine.Random.RandomRange(0, frameRates.Count);
            frameRates.Insert(1, frameRates[randomIndex]);

            Debug.Log("Frame rates: " + string.Join(", ", frameRates));
		}
		// If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

    public void ResetPlayerStats(bool resetNumberOfTries)
    {
        damageTaken = 0.0f;
        timeOfCompletionInSeconds = 0.0f;
        shotsFired = 0.0f;
        shotsHit = 0.0f;
        headShotsHit = 0.0f;
        accuracy = 0.0f;
        headShotAccuracy = 0.0f;
        maxHeadShotStreak = 0;
        isInHeadshotStreak = false;
        headShotStreakCount = 0;
        totalHeadShotDistance = 0;
        totalClosestEnemyDistance = 0;
        averageHeadShotDistance = 0;
        averageClosestEnemyDistance = 0;
        finishedRoundSuccessfully = false;
        totalTime = 0.0f;
        totalRotation = 0;
        averageRotationRate = 0;
        overallDistanceTraveled = 0.0f;

        if (resetNumberOfTries)
        {
            numberOfSpeedrunTry = 0;
            playerPerformanceExporter.DeleteCSVFiles();
        }
    }  
    public void DisplayPlayerStats()
    {
        TimeSpan ts = TimeSpan.FromSeconds(timeOfCompletionInSeconds);
        accuracy = (shotsHit / shotsFired) * 100.0f;
        headShotAccuracy = (headShotsHit / shotsHit) * 100.0f; 
        averageHeadShotDistance = totalHeadShotDistance / headShotsHit;
        averageRotationRate = totalRotation / totalTime;
        
        playerPerformanceExporter.ExportPlayerStatsToCSV(damageTaken, timeOfCompletionInSeconds, shotsFired, shotsHit,
        headShotsHit, accuracy, headShotAccuracy, maxHeadShotStreak, averageHeadShotDistance,
         averageRotationRate, overallDistanceTraveled, finishedRoundSuccessfully);
        PlayerPerformanceManager.Instance.ResetPlayerStats(false);
    }

    public void UpdateDamageTaken(float damageTaken)
    {
        this.damageTaken += damageTaken;
    }

    public void UpdateTimeOfCompletion(float timeOfCompletion)
    {
        this.timeOfCompletionInSeconds = timeOfCompletion;
    }

    public void UpdateShotsFired()
    {
        shotsFired++;
    }

    public void UpdateShotsHit()
    {
        shotsHit++;
    }

    public void UpdateHeadShotsHit()
    {
        headShotsHit++;
        UpdateHeadShotStreak(true);
    }
    
    public void UpdateHeadShotStreak(bool isHeadShot)
    {
        isInHeadshotStreak = isHeadShot;

        if (isInHeadshotStreak)
        {
            headShotStreakCount++;
            if (headShotStreakCount > maxHeadShotStreak)
            {
                maxHeadShotStreak = headShotStreakCount;
            }
        }
        else
        {
            headShotStreakCount = 0;
        }
    }

    public void UpdateHeadShotDistance(float distance)
    {
        totalHeadShotDistance += distance;
    }

    public void UpdateManagedToFinishRound(bool managedToFinishRound)
    {
        finishedRoundSuccessfully = managedToFinishRound;
    }

    public void UpdateTotalRotation(float rotation)
    {
        totalRotation = rotation;
    }

    public void UpdateTotalTime(float deltaTime)
    {
        totalTime = deltaTime;
    }

    public void UpdateOverallDistanceTraveled(float distanceCovered)
    {
        overallDistanceTraveled = distanceCovered;
    }
}