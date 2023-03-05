using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private int levelSelectScene = 2;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject winnerDisplay;
    [SerializeField] private TextMeshProUGUI winnerName;
    [SerializeField] private GameObject gameWinnerDisplay;
    [SerializeField] private TextMeshProUGUI gameWinnerName;
    [SerializeField] private GameObject three;
    [SerializeField] private GameObject two;
    [SerializeField] private GameObject one;
    private int numPlayers;
    private int numDeadPlayers;
    private List<int> livingPlayers = new List<int>();
    private Player winner = null;
    private bool gameOver = false;
    private GameSounds gameSounds;

    // Start is called before the first frame update
    void Awake()
    {
        // Places every character
        gameSounds = GetComponentInChildren<GameSounds>();
        StartCoroutine(PlaceAllCharacters());
    }

    // Coroutine and ienumerator allows waiting within function (for game begin countdown)
    IEnumerator PlaceAllCharacters()
    {
        List<Player> allPlayers = PersistentValues.persistentValues.players;
        foreach (Player player in allPlayers)
        {
            playerManager.placePlayer(player.playerInput, player.playerNum);
            // Make sure all players are alive again.
            PlayerGameplay playerGameplay = player.playerInput.GetComponent<PlayerGameplay>();
            playerGameplay.NewMatch();
            playerGameplay.SetGameController(this);
            playerGameplay.SetPlayerNum(player.playerNum);
            livingPlayers.Add(player.playerNum);
            // TODO tell players they are spawning for when they have spawn VFXs or animations here
        }


        // DISPLAY COUNTOWN ON UI TODO
        three.SetActive(true);
        gameSounds.PlayCountdownSound();
        yield return new WaitForSeconds(1);
        // Make the crowd clap as players spawn IF there is any crowd:
        makeCrowdClap();
        three.SetActive(false);
        two.SetActive(true);
        gameSounds.PlayCountdownSound();
        yield return new WaitForSeconds(1);
        two.SetActive(false);
        one.SetActive(true);
        gameSounds.PlayCountdownSound();
        yield return new WaitForSeconds(1);
        one.SetActive(false);
        gameSounds.PlayStartSound();
        // Unfreeze rigid body rotations

        // Make the crowd cheer as game starts IF there is any crowd:
        makeCrowdCheer();

        foreach (Player newPlayer in allPlayers)
        {
            Rigidbody rb = newPlayer.playerInput.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
        }
        PersistentValues.persistentValues.gameIsStarted = true;
    }

    public void ReportDeath(int playerNum)
    {
        livingPlayers.Remove(playerNum);
        // Make the crowd clap when someone dies IF there is any crowd:
        makeCrowdClap();
    }

    // Update is called once per frame
    void Update()
    {
        // Determine when game is over
        // if (one or 0 players left alive) end game
        if (!gameOver && livingPlayers.Count <= 1)
        {
            gameOver = true;
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        if (livingPlayers.Count == 1)
        {
            // Make the crowd cheer when someone wins IF there is any crowd:
            if (PersistentValues.persistentValues.crowd.Count > 0) makeCrowdCheer();
            winner = PersistentValues.persistentValues.players[livingPlayers[0]];
            PersistentValues.persistentValues.AddPlayerVictory(livingPlayers[0]);
            gameSounds.PlayVictorySound();
        }
        // End game 

        // If playing to certain number of games, check if any players has reached the number,
        // If they have display the winning scene rather than the levelSelectScene with the winner showcased
        if (PersistentValues.persistentValues.numGamesToWin != 0)
        {
            List<Player> allPlayers = PersistentValues.persistentValues.players;
            foreach (Player player in allPlayers)
            {
                if (player.score >= PersistentValues.persistentValues.numGamesToWin)
                {
                    // TODO Create a scene just for the winner, can have fireworks and a few
                    // seconds for them to celebrate, make the losers all sit in a box or something.
                    gameWinnerDisplay.SetActive(true);
                    gameWinnerName.text = player.selectedCharacter.gameObject.name;
                    gameWinnerName.color = player.selectedCharacter.gameObject.GetComponent<SkinInfo>().GetColor();
                    yield return new WaitForSeconds(1);
                    if (winner != null) winner.playerInput.gameObject.GetComponent<PlayerGameplay>().playerAudio.playWinSound();
                    yield return new WaitForSeconds(7);
                    PersistentValues.persistentValues.gameIsStarted = false;
                    // Need to remove all "do not destroy items" AKA the players
                    foreach (Player onePlayer in PersistentValues.persistentValues.players)
                    {
                        Destroy(onePlayer.playerInput.gameObject);
                    }
                    SceneManager.LoadScene(0); // Back to Main Menu
                }
            }
        }

        // DISPLAY WINNER ON UI
        ShowWinner();
        yield return new WaitForSeconds(1);
        if (winner != null) winner.playerInput.gameObject.GetComponent<PlayerGameplay>().playerAudio.playWinSound();
        yield return new WaitForSeconds(4);
        // end game essentially and move to level select:
        PersistentValues.persistentValues.gameIsStarted = false;
        // want a quick Victory pop up here before returning to scene selection so will need coroutines probably. 
        SceneManager.LoadScene(levelSelectScene);
    }

    public void ShowWinner()
    {
        // Enable winnerDisplay on GameUI so winner's name appears
        winnerDisplay.SetActive(true);
        if (winner != null)
        {
            winnerName.text = winner.selectedCharacter.gameObject.name;
            // Would be great to change WinnerName color depending on player
            SkinInfo skinInfo = winner.selectedCharacter.gameObject.GetComponent<SkinInfo>();
            if (skinInfo != null) winnerName.color = skinInfo.GetColor();
        } else
        {
            winnerName.text = "No One";
            winnerName.color = Color.black;
        }
        

        //Invoke("HideWinner", 3);

    }

    public void HideWinner()
    {
        // Maybe call this in this class after a certain amount of time, or call it upon death from death and respawn. 
        winnerDisplay.SetActive(false);
    }

    private void makeCrowdCheer()
    {
        if (PersistentValues.persistentValues.crowd.Count == 0) return;
        foreach (CrowdController crowdMember in PersistentValues.persistentValues.crowd)
        {
            crowdMember.beginCheer();
        }
    }

    private void makeCrowdClap()
    {
        if (PersistentValues.persistentValues.crowd.Count == 0) return;
        foreach (CrowdController crowdMember in PersistentValues.persistentValues.crowd)
        {
            crowdMember.beginClap();
        }
    }
}
