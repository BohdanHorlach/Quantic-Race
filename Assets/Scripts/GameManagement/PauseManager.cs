using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PauseManager
{
    private static bool isPaused = false;

    public static void PauseGame()
    {
        isPaused = true;
        // Pause the game
        Time.timeScale = 0f;
        Debug.Log("Game Paused");
    }

    public static void ResumeGame()
    {
        isPaused = false;
        // Unpause the game
        Time.timeScale = 1f;
        Debug.Log("Game Resumed");
    }
}
