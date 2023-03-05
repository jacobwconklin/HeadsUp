using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    private PlayerGameplay gameplay;

    // Start is called before the first frame update
    void Start()
    {
        gameplay = GetComponentInParent<PlayerGameplay>();
    }

    // Update is called once per frame
    void Update()
    {
        // If head goes below 0 it also dies.
        if (gameplay.getAliveStatus() && transform.position.y < 0)
        {
            gameplay.headOnGround();
        }
    }

    // When head collides with something need to know if it is:
    // 1) The ground (Player loses)
    // 2) another player (taking a hit)
    // 3) mundane obstacle / wall (do nothing)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("GROUND"))
        {
            gameplay.headOnGround();
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("GROUND"))
        {
            gameplay.headOnGround();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            // maybe have headbuts exert a force?
        }
    }
}
