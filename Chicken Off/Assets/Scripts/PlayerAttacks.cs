using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacks : MonoBehaviour
{
    // Describes the max distance that a player's push can reach
    [SerializeField] private float pushMaxDistance = 5.0f;
    // Amount of force behind a player's push
    [SerializeField] private float pushForce = 2.5f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private float pushPowerIncreaseIncrement = 1.0f;
    public float increasedPushPower = 0.0f;
    //private RaycastHit hitInfo;
    private PlayerGameplay playerGameplay;

    // TODO maybe combine kick and push cooldowns so they can't be done simultaneously? maybe it doesn't matter.

    // Push Values
    // amount of time required for push to cooldown
    [SerializeField] private float pushCooldown = 1.0f;
    private float nextPushTime = 0;
    private bool pressedPush = false;

    // Kick Values
    [SerializeField] private float kickCooldown = 1.0f;
    private float nextKickTime = 0;
    private bool pressedKick = false;

    // Start is called before the first frame update
    void Start()
    {
        playerGameplay = GetComponent<PlayerGameplay>();
    }

    public void resetAttackPower()
    {
        increasedPushPower = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Perform a push forwards
        // needs a cooldown entire animation at least should execute
        if (pressedPush && Time.time >= nextPushTime)
        {
            // wait a third of a second so animation reaches full extent
            // Show animation regardless
            playerGameplay.animator.SetTrigger("Push");
            Invoke("ForcePush", 0.3f);
            // Cue Push Sound
            playerGameplay.playerAudio.playHitSound();
            nextPushTime = Time.time + pushCooldown;
        }

        // Perform a kick. Kick should immediately rotate opponent hit backwards and push them
        // up and out into the air a bit? 
        /*
        if (pressedKick && Time.time >= nextKickTime)
        {
            // wait 0.2f seconds so animation lines up
            playerGameplay.animator.SetTrigger("Kick");
            Invoke("Kick", 0.2f);
            // Only have kick sound on making contact?
            nextKickTime = Time.time + kickCooldown;
        }
        */

    }

    // input receiving methods
    public void OnPush(InputAction.CallbackContext pushed) => pressedPush = pushed.ReadValueAsButton();
    public void OnKick(InputAction.CallbackContext kicked) => pressedKick = kicked.ReadValueAsButton();

    // Perform special attack unique to each player? Or perform spin? -> Just ideas for more actions and inputs. 

    // Attack Methods:
    void Kick()
    {
        // Only allow when game is started
        if (PersistentValues.persistentValues == null || !PersistentValues.persistentValues.gameIsStarted) return;

        // Try to find enemy player just a bit forward and up from current player. Any enemies connected with
        // will receive a rotation force and an uprwards and outwards force. 
        Collider[] collidingPlayers = new Collider[100];
        // Only want to kick each player once.
        List<GameObject> kickedPlayers = new List<GameObject>();
        int numOfCollidersDetected = Physics.OverlapSphereNonAlloc((transform.position + new Vector3(0, 0.3f, 0.75f)),
            attackRadius, collidingPlayers, 1 << 7);
        // Loop through collidingPlayers
        for (int i = 0; i < numOfCollidersDetected; i++)
        {
            bool landedHit = false;
            // ignore self and don't push the same players multiple times
            if (collidingPlayers[i].gameObject == gameObject || kickedPlayers.Contains(collidingPlayers[i].gameObject)) continue;
            // collidingPlayers[i] same checks and logic as raycast version
            Rigidbody collidedRigidBody = collidingPlayers[i].gameObject.GetComponent<Rigidbody>();
            if (collidedRigidBody != null)
            {
                Vector3 pushDirection = transform.forward + transform.up;
                pushDirection.Normalize();
                // extra / 2 so kick force is half of pushing force since it has the added rotational force
                collidedRigidBody.AddForce(pushDirection * (pushForce + increasedPushPower) / 2, ForceMode.VelocityChange);
                // Add rotational force
                // Need a Quaternion rot get it
                // Quaternion newRotation = Quaternion.Euler(collidedRigidBody.position +  (transform.forward * 20) + (transform.up * -20));
                // collidedRigidBody.MoveRotation(collidedRigidBody.rotation * newRotation);
                // Move rotation just moves it instantly, instead I should use add Torque
                collidedRigidBody.AddTorque(transform.forward * 3000);// , ForceMode.Force);


                // Check that enemy has a PlayerGameplay component and if they
                // do report the hit
                PlayerGameplay enemyGameplay = collidedRigidBody.gameObject.GetComponent<PlayerGameplay>();
                if (enemyGameplay != null && enemyGameplay.getAliveStatus())
                {
                    kickedPlayers.Add(collidingPlayers[i].gameObject);
                    // Successfully hit lving an enemy, increase attack power.
                    // increasedPushPower += pushPowerIncreaseIncrement;
                    // enemyGameplay.takeAHit();


                    // Could effectively change hitEffects transform here to occur right where hits happen
                    // And make one hitEffect for each hit
                    landedHit = true;
                }
            }

            if (landedHit)
            {
                // Create visual for kick and sound for kick here
                // playerGameplay.hitEffect.CreateKickEffect();
            }
        }
    }

    void ForcePush()
    {
        // Only allow when game is started
        if (PersistentValues.persistentValues == null || !PersistentValues.persistentValues.gameIsStarted) return;

        // TODO try using Physics.OverlapSphereNonAlloc for a big sphere area to push all players in rather than raycast
        // also better would be to spawn hitEffects AT THE LOCATION where the collisions take place but for now hitEffect
        // can match overlap sphere. 
        // Store results (aka all those collided with here)
        Collider[] collidingPlayers = new Collider[100];
        // Only want to push each player once.
        List<GameObject> pushedPlayers = new List<GameObject>();
        // Need a layer just for 
        int numOfCollidersDetected = Physics.OverlapSphereNonAlloc((transform.position + new Vector3(0, 1.5f, 0.75f)), 
            attackRadius, collidingPlayers, 1 << 7);
        // Loop through collidingPlayers
        for (int i = 0; i < numOfCollidersDetected; i++)
        {
            bool landedHit = false;
            // ignore self and don't push the same players multiple times
            if (collidingPlayers[i].gameObject == gameObject || pushedPlayers.Contains(collidingPlayers[i].gameObject)) continue;
            // collidingPlayers[i] same checks and logic as raycast version
            Rigidbody collidedRigidBody = collidingPlayers[i].gameObject.GetComponent<Rigidbody>();
            if (collidedRigidBody != null)
            {
                Vector3 pushDirection = transform.forward;
                pushDirection.Normalize();
                // for some reason pushes are so powerful? so Im going to decrease this vector substantially
                collidedRigidBody.AddForce(pushDirection * (pushForce + increasedPushPower), ForceMode.VelocityChange);
                // Check that enemy has a PlayerGameplay component and if they
                // do report the hit
                PlayerGameplay enemyGameplay = collidedRigidBody.gameObject.GetComponent<PlayerGameplay>();
                if (enemyGameplay != null && enemyGameplay.getAliveStatus())
                {
                    pushedPlayers.Add(collidingPlayers[i].gameObject);
                    // Successfully hit lving an enemy, increase attack power.
                    increasedPushPower += pushPowerIncreaseIncrement;
                    enemyGameplay.takeAHit();

                    // Could effectively change hitEffects transform here to occur right where hits happen
                    // And make one hitEffect for each hit
                    landedHit = true;
                }
            }

            if (landedHit)
            {
                // Create visual for hit
                playerGameplay.hitEffect.CreateHitEffect();
            }
        }

        /* Raycast version replaced with OverlapSphereNonAlloc
        Vector3 launchOrigin = transform.position;
        launchOrigin.y += 1.5f;
        Debug.Log("force pushing new raycast launch origin is:\n x:" + launchOrigin.x + " y: " + launchOrigin.y + " z:" + launchOrigin.z);

        if (Physics.Raycast(launchOrigin, transform.forward, out hitInfo, pushMaxDistance))
        {
            // Check that an ememy with a rigid body has been hit. If so apply the
            // force to the rigid body
            if (hitInfo.rigidbody)
            {
                Debug.Log("push found rigidbody: " + hitInfo.rigidbody.gameObject.name);
                Vector3 pushDirection = transform.forward;
                pushDirection.Normalize();
                // for some reason pushes are so powerful? so Im going to decrease this vector substantially
                // pushDirection = pushDirection * 0.0001f;
                // hitInfo.rigidbody.AddForceAtPosition(pushDirection * (pushForce - (hitInfo.distance / 100)), hitInfo.rigidbody.transform.position);
                hitInfo.rigidbody.AddForce(pushDirection * (pushForce + increasedPushPower), ForceMode.VelocityChange);
                // Check that enemy has a PlayerGameplay component and if they
                // do report the hit
                PlayerGameplay enemyGameplay = hitInfo.rigidbody.gameObject.GetComponent<PlayerGameplay>();
                if (enemyGameplay != null && enemyGameplay.getAliveStatus()) 
                {
                    // Successfully hit lving an enemy, increase attack power.
                    increasedPushPower += pushPowerIncreaseIncrement;
                    enemyGameplay.takeAHit();
                    // Create visual for hit
                    playerGameplay.hitEffect.CreateHitEffect();
                }
            }
        } */
    }
}
