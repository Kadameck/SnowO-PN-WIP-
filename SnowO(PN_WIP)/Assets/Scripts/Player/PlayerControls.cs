using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls all actions that the user can have the main character perform
/// </summary>
public class PlayerControls : MonoBehaviour
{
    #region Visible in the Unity Inspector
    // The Pausescreen script to check if the game is paused or not
    [SerializeField]
    private Pausescreen pause;

    // The BridgeSkillTrigger Script to check if the user can usw the Skill to create a bridge
    [SerializeField]
    private BridgeSkillTrigger bridgeSkill;

    [SerializeField]
    private GameObject[] bridgeSkillPrefabs = new GameObject[2];

    // The impulse down at the highest point of a jump so that the main character falls faster to make the jump look more realistic
    [SerializeField]
    [Tooltip("The impulse down at the highest point of a jump so that the main character falls faster to make the jump look more realistic (Default 10)")]
    private float fallSpeed = 10;

    // The speed of movement of the main character
    [SerializeField]
    [Tooltip("The speed of movement of the main character (Default 5)")]
    private int moveSpeed = 5;

    // The jumpforce (jump height and width) of the main character
    [SerializeField]
    [Tooltip("The jumpforce (jump height and width) of the main character (Default 300)")]
    private int jumpForce = 300;

    // The force with which the main character slips off an edge if it protrudes too far beyond it
    [SerializeField]
    [Tooltip("The force with which the main character slips off an edge if it protrudes too far beyond it (Default 10)")]
    private int edgeSlippingForce = 10;
    #endregion

    // The PlayerGroundDetect-Script
    private PlayerGroundDetect groundDetect;
    // The Rigidbody2D component of the main character
    private Rigidbody2D rb;
    // The direction of the Movement on the x-Axis
    private float xMove;

    /// <summary>
    /// Initialized all variables and states of the main character in the first frame of the game
    /// </summary>
    void Start()
    {
        // Initialized the PlayerGroundDetect-Script in the groundDetect variable
        groundDetect = GetComponent<PlayerGroundDetect>();
        // Initialized the Rigidbody2D in the rb variable
        rb = GetComponent<Rigidbody2D>();

        // A Debugging information in the Unity console if at least 1 prefab is missing
        if(bridgeSkillPrefabs[0] == null || bridgeSkillPrefabs[1] == null)
        {
            Debug.Log("Missing at least 1 bridgeSkillPrefab");
        }

        // A Debugging information in the Unity console if the bridgeSill script is missing
        if (bridgeSkill == null)
        {
            Debug.Log("Missing BridgeSkillTrigger script");
        }
    }

    /// <summary>
    /// Loops though all funktions once per frame
    /// </summary>
    void Update()
    {
        // If the game ist currently not paused
        if(!pause.GetIsPaused())
        {
            // Calls the "Movement()" funktion
            Movement();
            // Calls the "Jump()" funktion
            Jump();
            // Calls the "SpecialSkills()" funktion
            SpecialSkills();
            // Calls the "Fall()" funktion
            Fall();
        }
    }

    /// <summary>
    /// Controls the Horizontal movement of the main character
    /// </summary>
    private void Movement()
    {
        // Checks whether the main character is on the floor or something similar
        if (groundDetect.GetisGrounded())
        {
            // Recognizes whether the main character should move on the x axis
            xMove = Input.GetAxis("Horizontal");

            // Creates a tolerance of -0.1 to 0.1 with respect to horizontal movement
            // to ensure that the main character stops running when neither the A-Key nor the D-Key is pressed
            if (xMove > 0.1f && xMove < -0.1f)
            {
                // Sets xMove to 0 to stop the mein character movement on the x axis
                xMove = 0f;
            }

            // Checks whether the main character should move on the x-axis
            if (xMove != 0)
            {
                // Creates a movement along the x axis.
                // If xMove is less than 0 the main character moves to the left.
                // fIf xMove is greater than 0 the main character moves to the right.
                rb.velocity = new Vector2(xMove * moveSpeed, rb.velocity.y);
            }
        }
    }

    /// <summary>
    /// Allow the Player to Jump
    /// </summary>
    private void Jump()
    {
        // A jump is be possible if the user presses the spacebar while the main character is in contact with the ground
        if (groundDetect.GetisGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            // A jump is made when the rigidbody2D experiences an upward pulse. This impulse has the strength of "jumpForce"
            rb.AddForce(Vector2.up * jumpForce);
        }
    }

    /// <summary>
    /// Calls all SpecialSkills
    /// </summary>
    private void SpecialSkills()
    {
        // Calls the "CreatBridge()" funktion
        CreateBridge();
        
        // Funktions aufrufe für alle anderen fähigkeiten (D)
    }

    /// <summary>
    /// Allows the Player to use the good or evil skill to create a bridge on a abyss
    /// </summary>
    private void CreateBridge()
    {
        // Wenn der spieler im Trigger steht in dem eine brücke spawnbar sein soll...
        if (groundDetect.GetisGrounded() && Input.GetKeyDown(KeyCode.E) && bridgeSkill.GetIsUsable())
        {

            // Wird jeweils unter dem spieler ein raycast nach links und nach rechts geschossen
            // diese beiden raycasts ignorieren alle layer bis auf layer 8 welche die "schluchtrand collider" sind
            RaycastHit2D leftRay = Physics2D.Raycast(
                transform.position - new Vector3(0, GetComponent<BoxCollider2D>().bounds.size.y / 2 + 0.5f, 0),
                Vector2.left,
                5,
                1 << 8); // Notiz: Es gibt 32 Layer dh 32 bits. mit diesem befehl wird gesagt "ersetzte den 8 von 32 bits mit einer 1" diese 1 bedeutet "true". das ist bitshifting
                         // eine 0 bedeutet false (zB 0 << 9 würde den 9 layer unsichtbar machen. würde aber keinen sinn machen da standartmäßig alle layer auf 0 sind wenn man keine angabe macht)

            RaycastHit2D rightRay = Physics2D.Raycast(
                transform.position - new Vector3(0, GetComponent<BoxCollider2D>().bounds.size.y / 2 + 0.5f, 0),
                Vector2.right,
                5,
                1 << 8);

            // Wenn der nach links gehende raycast einen "schluchtrand collider" trifft...
            if (leftRay.collider != null)
            {
                // Wird auf der linken seite vom spieler eine brücke erscheinen
                GameObject.Instantiate(bridgeSkillPrefabs[0], transform.position - new Vector3(3, 1, 0), Quaternion.identity); // Die anpassung wo die brücke erscheinen soll und in welche richtung die verläuft muss noch gemacht werden
            }
            // Wenn der nach rechts gehende raycast einen "schluchtrand collider" trifft...
            else if (rightRay.collider != null)
            {
                // Wird auf der rechten seite vom spieler eine brücke erscheinen
                GameObject.Instantiate(bridgeSkillPrefabs[0], transform.position - new Vector3(3, 1, 0), Quaternion.identity); // Die anpassung wo die brücke erscheinen soll und in welche richtung die verläuft muss noch gemacht werden
            }
        }
    }

    /// <summary>
    /// Accelerates the fall of the main character to make it more realistic and ensures that the main character slips off edges
    /// </summary>
    private void Fall()
    {
        // Determines whether the main character should fall in a certain direction from an edge
        if (groundDetect.GetFallDirection()[0] || groundDetect.GetFallDirection()[1])
        {
            // When the main character slips from an edge to the left, a force is exerted to the left
            if (groundDetect.GetFallDirection()[0])
            {
                rb.AddForce(Vector2.left * edgeSlippingForce);
            }
            // When the main character slips from an edge to the right, a force is exerted to the right
            else if (groundDetect.GetFallDirection()[1])
            {
                rb.AddForce(Vector2.right * edgeSlippingForce);
            }
        }

        // Checks whether there is no ground contact and the main character is in free fall
        if (!groundDetect.GetisGrounded() && rb.velocity.y < 0)
        {
            // Gives the main character a downward impulse with the strength of "FallSpeed"
            rb.AddForce(Vector2.down * fallSpeed);
        }
    }

}
