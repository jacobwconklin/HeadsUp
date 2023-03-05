
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Collections;

/**
 * STRUCTS
 */

public class Player
{
    public GameObject selectedCharacter;
    public AvailableNode currSelection; // Used as a cursor / iterator of sorts
    public int score;
    public PlayerInput playerInput;
    public int playerNum;
}

public class Skin
{
    public Vector3 originalLocation;
    public Quaternion originalRotation;
    public string name;
}

public class PersistentValues
{
    // Use game is started to allow player movement
    public bool gameIsStarted = false;
    //public bool beSilent = false; // Makes player audios quiet during level select or other times. 

    public bool canSelectCharacters = false;

    public static PersistentValues persistentValues;

    public int numPlayers = 0;

    // Set by players to determine how many wins to finish. Default to 0 which
    // means there is no limit, or classic endless mode. 
    public float numGamesToWin = 0;

    public List<Player> players; // A list of player structs keeping all of their necessary info
    // Plan to even keep track of player's cosmetic choices here 
    // Selected Characters are moved to this List and removed form available Characters list

    // Permanent list of Selectable Characters with corresponding names and skins
    public AvailableLinkedList availableCharacters;

    // Don't have access to a map unfortunately :( but should be relatively small amount of
    // available skins so just save names and initial locations. 
    private List<Skin> skins;

    // Levels that have a Crowd will populate this list themselves by the crowd objected directly
    // inserting themselves and removing themselves when that level ends. 
    public List<CrowdController> crowd;

    public static void Initialize(List<GameObject> selectableCharacters)
    {
        persistentValues = new PersistentValues();
        persistentValues.players = new List<Player>();
        persistentValues.crowd = new List<CrowdController>();
        persistentValues.availableCharacters = new AvailableLinkedList(selectableCharacters);

        persistentValues.skins = new List<Skin>();
        // Manually put in all character skinNames to locations into map
        foreach(GameObject skin in selectableCharacters)
        {
            persistentValues.skins.Add(new Skin
            {
                name = skin.name,
                originalLocation = skin.transform.position,
                originalRotation = skin.transform.rotation
            }) ;
        }
    }

    public void Reset()
    {
        numPlayers = 0; // And resets the number of Players, leave player's skin and flag selections for now. 

        // Don't want to just erase this because I want to preserve player skin choices, but for now I will just reset it. 
        players = new List<Player>();
        persistentValues.crowd = new List<CrowdController>();
        availableCharacters.setAllAvailable();
        gameIsStarted = false;
    }


    // returns number telling the player that it is the nth player to join 
    // Needs to be called when players are created. 
    public int AddPlayer(PlayerInput playerInput)
    {
        numPlayers++; 
        Player newPlayer = new Player();
        newPlayer.playerInput = playerInput;
        newPlayer.currSelection = availableCharacters.getFirstAvailable();
        newPlayer.selectedCharacter = newPlayer.currSelection.value;
        // SET NEW SKIN AS CHILD OF PLAYERINPUT AND SET PLAYERINPUT'S PLAYERMOVMENT ANIMATOR TO THE NEW SKIN
        newPlayer.selectedCharacter.transform.SetParent(newPlayer.playerInput.transform);
        // MANUALLY SET POSITION AND ROTATION?
        newPlayer.selectedCharacter.transform.position = newPlayer.playerInput.transform.position;
        newPlayer.selectedCharacter.transform.rotation = newPlayer.playerInput.transform.rotation;
        Animator newSkinAnimator = newPlayer.selectedCharacter.GetComponent<Animator>();
        PlayerGameplay playerGameplay = newPlayer.playerInput.GetComponent<PlayerGameplay>();
        playerGameplay.animator = newSkinAnimator;
        playerGameplay.playerAudio = newPlayer.selectedCharacter.GetComponentInChildren<PlayerAudio>();
        playerGameplay.hitEffect = newPlayer.selectedCharacter.GetComponentInChildren<HitEffect>();
        // NEED TO DO SOMETHING TO ANIMATOR TO SET UP JUMP TRIGGERING CORRECTLY FOR SOME REASON IDK WHAY THE PROB IS 
        playerGameplay.playerNum = numPlayers - 1;
        newPlayer.playerNum = numPlayers - 1;

        newPlayer.score = 0;
        players.Add(newPlayer);
        return numPlayers;
    }

    public void NextSkin(int playerNum)
    {
        Player player = players[playerNum];
        // NEED TO REMOVE PARENT??
        Animator oldSkinAnimator = player.selectedCharacter.GetComponent<Animator>();
        player.selectedCharacter.transform.SetParent(null);
        // PUT CHARACTER SKINS "BACK" BASED ON CHARACTER SKIN NAME 
        // Alternatively could just throw unselected skin under stage (easy) --> player.selectedCharacter.transform.position = new Vector3(0, -50, 0);
        foreach (Skin skin in skins)
        {
            if (skin.name == player.selectedCharacter.name)
            {
                player.selectedCharacter.transform.position = skin.originalLocation;
                player.selectedCharacter.transform.rotation = skin.originalRotation;
                break;
            }
        }
            

        player.currSelection = availableCharacters.nextAvailable(player.currSelection);
        player.selectedCharacter = player.currSelection.value;
        // SET NEW SKIN AS CHILD OF PLAYERINPUT AND SET PLAYERINPUT'S PLAYERMOVMENT ANIMATOR TO THE NEW SKIN
        player.selectedCharacter.transform.SetParent(player.playerInput.transform);
        // MANUALLY SET POSITION AND ROTATION?
        player.selectedCharacter.transform.position = player.playerInput.transform.position;
        player.selectedCharacter.transform.rotation = player.playerInput.transform.rotation;
        Animator newSkinAnimator = player.selectedCharacter.GetComponent<Animator>();
        PlayerGameplay playerGameplay = player.playerInput.GetComponent<PlayerGameplay>();
        playerGameplay.animator = newSkinAnimator;
        playerGameplay.playerAudio = player.selectedCharacter.GetComponentInChildren<PlayerAudio>();
        playerGameplay.hitEffect = player.selectedCharacter.GetComponentInChildren<HitEffect>();
    }

    public void AddPlayerVictory(int playerNum) // When a player wins add them based on their index in the playerName list
    {
        // Get name from game object?
        Player player = players[playerNum];
        player.score++;
    }

}