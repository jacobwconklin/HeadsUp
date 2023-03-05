using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFoot : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerGameplay playerGameplay;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerGameplay = GetComponentInParent<PlayerGameplay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        // TODO may want to only allow some kinds of collision to count as ground?
        // For now just don't count object itself
        
        playerMovement.isGrounded = true;
        if (playerGameplay.animator)
        {
            playerGameplay.animator.SetBool("IsGrounded", true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // TODO not sure how to tell if this foot is no longer touching anything?
        playerMovement.isGrounded = false;
        // Tell animator? 
        playerGameplay.animator.SetBool("IsGrounded", false);
    }

}
