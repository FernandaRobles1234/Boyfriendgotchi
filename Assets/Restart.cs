using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Restart : MonoBehaviour
{
    // Key to store the last restart time in PlayerPrefs
    private const string LastRestartTimeKey = "LastRestartTime";
    void Start()
    {
        CheckAndRestart();
    }

    private void CheckAndRestart()
    {
        // Get the current time
        DateTime currentTime = DateTime.Now;

        // Check if the key exists in PlayerPrefs
        if (PlayerPrefs.HasKey(LastRestartTimeKey))
        {
            // Get the last restart time
            string lastRestartTimeString = PlayerPrefs.GetString(LastRestartTimeKey);
            DateTime lastRestartTime = DateTime.Parse(lastRestartTimeString);

            // Calculate the time difference
            TimeSpan timeDifference = currentTime - lastRestartTime;

            // Check if 24 hours (1 day) has passed
            if (timeDifference.TotalHours >= 24)
            {
                RestartCurrentScene();
            }
        }
        else
        {
            // If key does not exist, set the current time as the last restart time
            PlayerPrefs.SetString(LastRestartTimeKey, currentTime.ToString());
        }
    }

    public void RestartCurrentScene()
    {
        // Update the last restart time to the current time
        PlayerPrefs.SetString(LastRestartTimeKey, DateTime.Now.ToString());

        // Get the active scene's name
        string sceneName = SceneManager.GetActiveScene().name;

        // Load the scene with the same name
        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        // Example: Restart the scene when the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartCurrentScene();
        }
    }
}
