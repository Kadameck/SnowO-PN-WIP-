using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that registers whether the player has contact with the ground or something comparable
/// </summary>
public class PlayerGroundDetect : MonoBehaviour
{
    // Notes whether the main character is in contact with the ground or something comparable or not
    private bool isGrounded = true;
    private bool fallToLeft = false;
    private bool fallToRight = false;

    // A layer mask to define a layer that will be ignored by the ground detection raycasts
    private LayerMask layerToIgnore = 1 << 8;

    /// <summary>
    /// Loops though all funktions once per frame
    /// </summary>
    void Update()
    {
        // Calls the GroundDetection() funktion
        GroundDetection();
    }

    /// <summary>
    /// Detects whether the main character is in contact with the ground or something similar
    /// </summary>
    private void GroundDetection()
    {
        // Shoots a raycast that originates on the left edge and 0.15 below the sprite and is shot from there to the right until the right edge of the sprite and ignored the layer 8
        RaycastHit2D leftRaycast = Physics2D.Raycast(
            (Vector2) transform.position - new Vector2(GetComponent<BoxCollider2D>().bounds.size.x / 2, GetComponent<BoxCollider2D>().bounds.size.y / 2 + 0.15f),
            Vector2.right,
            GetComponent<BoxCollider2D>().bounds.size.x,
            ~layerToIgnore);

        // Shoots a raycast that originates on the right edge and 0.15 below the sprite and is shot from there to the left until the left edge of the sprite and ignored the layer 8
        RaycastHit2D rightRaycast = Physics2D.Raycast(
            (Vector2)transform.position + new Vector2(GetComponent<BoxCollider2D>().bounds.size.x / 2, -GetComponent<BoxCollider2D>().bounds.size.y / 2 - 0.15f),
            Vector2.left,
            GetComponent<BoxCollider2D>().bounds.size.x,
            ~layerToIgnore);

        // If both raycasts meet a collider, there is something under the main character on which it can possibly stand
        if (leftRaycast.collider != null && rightRaycast.collider != null)
        {
            // Calculates the distance on the x-axis between the two collision points of the raycasts and the object under the main character.
            // This indicates the width of the surface on which the main character would stand
            float hitObjectWidth = rightRaycast.point.x - leftRaycast.point.x;

            // If the surface under the main figure is at least minimally wider than half the main figure itself, 
            // the main figure can stand and thus has contact with the ground
            if (hitObjectWidth > GetComponent<BoxCollider2D>().bounds.size.x / 4 && !isGrounded)
            {
                isGrounded = true;
                fallToLeft = fallToRight = false;
            }

            // If the surface under the main character is only a quarter as wide as the main character or even less, there is no ground contact
            else if (hitObjectWidth <= GetComponent<BoxCollider2D>().bounds.size.x / 4 && isGrounded)
            {
                isGrounded = false;

                // Calculates which raycast is longer, the left or the right. This tells us whether the main character on the left or right has lost contact with the ground
                float leftDist = Vector2.Distance((Vector2)transform.position - new Vector2(GetComponent<BoxCollider2D>().bounds.size.x / 2, GetComponent<BoxCollider2D>().bounds.size.y / 2 + 0.15f), leftRaycast.point);
                float rightDist = Vector2.Distance((Vector2)transform.position + new Vector2(GetComponent<BoxCollider2D>().bounds.size.x / 2, -GetComponent<BoxCollider2D>().bounds.size.y / 2 - 0.15f), rightRaycast.point);

                // If the left raycast is longer than the right one, the main character on the left has lost contact with the ground
                if (leftDist > rightDist)
                {
                    fallToRight = false;
                    fallToLeft = true;
                }
                // If the right raycast is longer than the left one, the main character on the right has lost contact with the ground
                else
                {
                    fallToLeft = false;
                    fallToRight = true;
                }
            }

        }
        // If the left or right raycast does not note a collision ...
        else
        {
            // If the player is still marked as "currently touching the ground" ......
            if (isGrounded)
            {
                // ... it will be changed to "no ground contact"
                isGrounded = false;
            }
        }
    }

    /// <summary>
    /// Allows other scripts to read the "isGrounded" variable
    /// </summary>
    /// <returns>The current state of the "isGrounded" variable</returns>
    public bool GetisGrounded()
    {
        return isGrounded;
    }

    public bool[] GetFallDirection()
    {
        bool[] fallDirection = new bool[2];
        fallDirection[0] = fallToLeft;
        fallDirection[1] = fallToRight;

        return fallDirection;
    }
}
