using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameplay : MonoBehaviour
{
    private bool isAlive = true;
    private GameController gameController = null;
    // Animator and player audio and player number get set by PersistentValues code
    public Animator animator;
    public PlayerAudio playerAudio;
    public HitEffect hitEffect;
    public int playerNum;


    // changes rigid body tilt max speed so they cant fall over as fast
    // TODO decreasing angular drag and increasing max angular velocity can make balancing harder could happen whenever a player takes a hit.
    private Rigidbody rb;
    [SerializeField] private float initalMaxAngVelocity = 0.80f;
    [SerializeField] private float initialAngularDrag = 0.8f;
    [SerializeField] GameObject balanceFoot;

    // For receiving input:
    private bool pressedSwitch = false;

    /*
     *  Track important values for Gameplay and gameplay events occurring. 
     */
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Switch player skins while in character select stage
        if ( pressedSwitch && PersistentValues.persistentValues.canSelectCharacters)
        {
            PersistentValues.persistentValues.NextSkin(playerNum);
        }
        pressedSwitch = false;
    }

    // Reset important gameplay values
    public void NewMatch()
    {
        rb.ResetInertiaTensor();
        isAlive = true;
        // Resotre balance foot
        balanceFoot.SetActive(true);
        animator.SetBool("IsAlive", true);
        // Effect Rigid Body for more or less stability
        rb.maxAngularVelocity = initalMaxAngVelocity;
        rb.angularDrag = initialAngularDrag;
        // Reset increased pushing power
        GetComponent<PlayerAttacks>().resetAttackPower();
    }

    // Sets GameController for current scene to report death tod
    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

    // Sets GameController for current scene to report death tod
    public void SetPlayerNum(int playerNum)
    {
        this.playerNum = playerNum;
    }

    // Interaction methods here for SSOT for fields. 

    // Taking a hit may decrease degrees per second and movement speed which could be recovered
    // automatically over time. It may also display some effect?
    public void takeAHit()
    {
        // Make balancing harder
        rb.maxAngularVelocity = rb.maxAngularVelocity + 0.2f;
        rb.angularDrag = rb.angularDrag > 0.2f ? rb.angularDrag - 0.1f : 0.2f;
        // Play audio for getting hit
        playerAudio.playGetHitSound();
    }

    public bool getAliveStatus() { return isAlive; }

    // To be called by head's script when it collides with a layer designated as ground
    // (This is when a player should lose)
    public void headOnGround()
    {
        // Player loses Perform once per game:
        if (isAlive)
        {
            isAlive = false;
            // Cue death animation
            animator.SetBool("IsAlive", false);
            // Play death sound
            playerAudio.playDieSound();
            // Remove Balance foot so player less likely to stay upright
            balanceFoot.SetActive(false);
            if (gameController != null)
            {
                gameController.ReportDeath(playerNum);
            }
        }
    }

    public void OnSwitch(InputAction.CallbackContext switched) // This is set up for switching skins, need to change name and button and
    {
        if (switched.performed)
        {
            pressedSwitch = true;
        }
    }

    public bool getIsAlive() { return isAlive; }
}
