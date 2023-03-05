using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuickJoinTest : MonoBehaviour
{
    // private List<PlayerInput> players = new List<PlayerInput>();
    // private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> spawns;
    private PlayerInputManager inputManager;
    private int playerNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = FindObjectOfType<PlayerInputManager>();
    }

    /**
     * Called each time a new player clicks a button on their controller
     * thus entering the game
     */
    public void addPlayer(PlayerInput player)
    {
        player.transform.position = spawns[playerNum].position;
        player.transform.rotation = spawns[playerNum].rotation;
        playerNum++;
    }

}

