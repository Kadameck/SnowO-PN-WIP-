using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Pausescreen during gameplay
/// </summary>
public class Pausescreen : MonoBehaviour
{
    // Notes whether the game is paused or not
    private bool isPaused = false;

    /// <summary>
    /// Loops though all funktions once per frame
    /// </summary>
    private void Update()
    {
        // Calls the "TimeScaleControl()" funktion
        TimeScaleControl();
    }

    /// <summary>
    /// Controls the time scale of the game
    /// </summary>
    private void TimeScaleControl()
    {
        // if the user is pressing the P-Key and the game is not paused...
        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            // ... the timescale will be set to 0 to stop all current actions
            Time.timeScale = 0;
            //... the game will set to paused
            isPaused = !isPaused;
        }
        // // if the user is pressing the P-Key and the game is currently paused...
        else if (Input.GetKeyDown(KeyCode.P) && isPaused)
        {
            // ... the timescale will be set to 1 to continue all current actions
            Time.timeScale = 1;
            //... the game will set to not paused anymore
            isPaused = !isPaused;
        }
    }

    /// <summary>
    /// Allows other scripts to read the "isPaused" variable
    /// </summary>
    /// <returns>The current state of the "isPaused" variable</returns>
    public bool GetIsPaused()
    {
        return isPaused;
    }
}
