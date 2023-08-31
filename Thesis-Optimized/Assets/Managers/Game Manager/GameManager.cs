using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	// Singleton instance.
	public static GameManager Instance = null;
    private float mouseSensitivity;
	public float MouseSensitivity 
	{ 
		get
		{
			if (PlayerPrefs.HasKey("MouseSensitivity"))
			{
				mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
			}
			else
			{
				PlayerPrefs.SetFloat("MouseSensitivity", 1.0f);
				mouseSensitivity = 1.0f;
			}

			return mouseSensitivity;
		} 
		set 
		{
			PlayerPrefs.SetFloat("MouseSensitivity", value);
			mouseSensitivity = value;
		}
	}
    private float musicVolume;
	public float MusicVolume 
	{ 
		get
		{
			if (PlayerPrefs.HasKey("MusicVolume"))
			{
				musicVolume = PlayerPrefs.GetFloat("MusicVolume");
			}
			else
			{
				PlayerPrefs.SetFloat("MusicVolume", 1.0f);
				musicVolume = 1.0f;
			}
			
			return musicVolume;
		} 
		set 
		{
			PlayerPrefs.SetFloat("MusicVolume", value);
			musicVolume = value;
		}
	}

	private int targetFrameRate;
	public int TargetFrameRate
	{
		get
		{
			if (PlayerPrefs.HasKey("targetFrameRate"))
			{
				targetFrameRate = PlayerPrefs.GetInt("targetFrameRate");
			}
			else
			{
				PlayerPrefs.SetInt("targetFrameRate", 60);
				targetFrameRate = 60;
			}

			Application.targetFrameRate = targetFrameRate;
			return targetFrameRate;
		}
		set
		{
			PlayerPrefs.SetInt("targetFrameRate", value);
			targetFrameRate = value;
			Application.targetFrameRate = targetFrameRate;
		}
	}
	
	// Initialize the singleton instance.
	private void Awake()
	{
		// If there is not already an instance of SoundManager, set it to this.
		if (Instance == null)
		{
			Instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(gameObject);

		// PlayerPrefs.DeleteKey("MouseSensitivity");
		// PlayerPrefs.DeleteKey("MusicVolume");
		// PlayerPrefs.DeleteKey("targetFrameRate");
		// Debug.Log("MouseSensitivity: " + MouseSensitivity);

	}

	

	
	
}