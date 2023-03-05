using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 4.0f;
    [SerializeField] private float rotSpeed = 700f;
    [SerializeField] private float jumpForce = 50f;


    // As player gets "tired" or takes damage should decrease their degreesPerSecond that they can tilt
    [SerializeField] private float degreesPerSecond = 180.0f;

    // houses important information about player status
    private PlayerGameplay gameplay;

    // Foot to track when player is on the ground and allow them to move or not
    // private MoveFoot moveFoot; Caused problems where movement was prohibited in mid air or when tilted.
    // Instead I plan to effectively slow the player's movement the more they tilt based on the difference in
    // y values between their head and foot (to calculate their angle). 
    private float tiltReduction; // Reduces movement speed based on how much character is tilted. 
    [SerializeField] private Transform headLocation;

    // Where player Input is stored to be read by Update
    private Vector2 movementInput;
    private Vector2 tiltInput;
    // Jumping has a slight cooldown to fix glitchy jumps where collider stays inside 
    // other blocks and player jumps insanely high
    private float jumpCooldown = 0;
    private bool pressedJump = false;
    public bool isGrounded = true;
    public bool pressedStart = false; // in the future may want everyone to press start at same time to start game?


    // Start is called before the first frame update
    void Start()
    {
        gameplay = GetComponent<PlayerGameplay>();
        //moveFoot = GetComponentInChildren<MoveFoot>();
    }

    // Update is called once per frame
    void Update()
    {
        // While player is dead don't move
        if (!gameplay.getAliveStatus() || gameplay.animator == null)
        {
            return;
        }

        // If grounded and pressed jump perform jump. Animation should be triggered soley by colider?? maybe
        if (pressedJump && isGrounded && Time.time > jumpCooldown)
        {
            GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            // Cue sound if game is started
            if (!PersistentValues.persistentValues.gameIsStarted) gameplay.playerAudio.playJumpSound();
            jumpCooldown = Time.time + 0.5f;
        }

        if (movementInput != Vector2.zero)
        {

            /*
             * MOVE PLAYER
             */
            Vector3 velocity = new Vector3(movementInput.x, 0, movementInput.y);
            if (headLocation.position.y - transform.position.y > 1.8f) // At least 1.8 apart (head starts at 2.13)
            {
                // Mostly upright just move normally 
                tiltReduction = 1.0f;
            } else if (headLocation.position.y - transform.position.y > 0.5f) // now slow movement proportional to how tilted player is
            {
                tiltReduction = (headLocation.position.y - transform.position.y) / 2.0f;
            } else // Too tilted don't allow movement
            {
                tiltReduction = 0;
            }

                if (PersistentValues.persistentValues != null && PersistentValues.persistentValues.gameIsStarted)
            {
                // Compute x and z velocity based on speed and tilt reduction here so y velocity isn't impacted
                Vector3 computedVelocity = velocity * speed * tiltReduction;
                // Use player's same downward velocity when negative so gravity will continue to effect them while moving
                float yVelocity = GetComponent<Rigidbody>().velocity.y;
                if (yVelocity < 0) computedVelocity.y = GetComponent<Rigidbody>().velocity.y;
                transform.Translate(computedVelocity * Time.deltaTime , Space.World);
            }
            gameplay.animator.SetFloat("Speed", velocity.magnitude);

            // velocity.Normalize(); TODO trying to move / delete this line

            /*
             * ROTATE PLAYER
             * Allows player movement direction to turn Player along y axis
             */

            // Rotate towards direction of movemebt while moving
            // Now rotate LESS based on tilt, should help wild spinning when low
            // if (tiltReduction < 1) tiltReduction = tiltReduction / 8; could reducing turning by a high factor or
            // just disable it entirely past a certain angle, which I am currently choosing
            if (headLocation.position.y - transform.position.y > 2.0f) // At least 1.8 apart (head starts at 2.13)
            {
                // Mostly upright just rotate normally 
                tiltReduction = 1.0f;
            }
            else if (headLocation.position.y - transform.position.y > 1.5f) // now slow rotation proportional to how tilted player is
            {
                tiltReduction = (headLocation.position.y - transform.position.y) / 16.0f;
            }
            else // Too tilted don't allow movement
            {
                tiltReduction = 0;
            }
            velocity.y = transform.position.y;
            Quaternion toRotate = Quaternion.LookRotation(velocity, Vector3.up);
            // Movement causes rotation soley around y axis and leaves x / z rotation up to
            // player input. 
            Quaternion newRot = Quaternion.RotateTowards(transform.rotation, toRotate, rotSpeed * tiltReduction * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, newRot.eulerAngles.y, transform.eulerAngles.z);
        } else
        {
            gameplay.animator.SetFloat("Speed", 0);
        }

        /*
         * TILT PLAYER
         * Allow player to tilt their character to maintain its balance
         * IE rotation along x and z axis
         */
        if (tiltInput != Vector2.zero)
        {
            if (PersistentValues.persistentValues != null && PersistentValues.persistentValues.gameIsStarted)
            {
                transform.Rotate(tiltInput.y * degreesPerSecond * Time.deltaTime,
                    0, tiltInput.x * -1 * degreesPerSecond * Time.deltaTime, Space.World);
            }
        }
    }

    // Input receiving methods
    public void OnMove(InputAction.CallbackContext moveValue) => movementInput = moveValue.ReadValue<Vector2>();
    public void OnTilt(InputAction.CallbackContext tiltValue) => tiltInput = tiltValue.ReadValue<Vector2>();
    public void OnJump(InputAction.CallbackContext jumped) => pressedJump = jumped.ReadValueAsButton(); 
    public void OnStart(InputAction.CallbackContext started) => pressedStart = started.ReadValueAsButton();
}
