using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the TriggerBoxes that alow the Player to use the BridgeSkill
/// </summary>
public class BridgeSkillTrigger : MonoBehaviour
{
    // A Variable to return to the PlayerControls to determine if the player can use the BridgeSkill
    private bool isUsable;

    /// <summary>
    /// Sets the "bridgeSkill" to usable when the player is in a trigger
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        // Checks whether the player is in the trigger
        if (collision.CompareTag("Player"))
        {
            // Sets the bridgeSkill to usable
            isUsable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Checks whether the player is leaving the trigger
        if (collision.CompareTag("Player"))
        {
            // Sets the bridgeSkill to unusable
            isUsable = false;
        }
    }


    /// <summary>
    /// Returns the current status of the isUsable variable
    /// </summary>
    /// <returns>The current status of the bridgeSkill usability</returns>
    public bool GetIsUsable()
    {
        return isUsable;
    }
}
