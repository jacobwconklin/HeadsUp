
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelSelection : MonoBehaviour
{
    [SerializeField] private int firstMapSceneNumber = 3; // Important to set this right
    [SerializeField] private GameObject[] playerScoreObjects;

    private void Start()
    {
        // Display some scores!
        DisplayScores();
    }

    private void DisplayScores()
    {
        int i = 0;
        foreach (Player player in PersistentValues.persistentValues.players)
        {
            // For each active player display their image and score
            playerScoreObjects[i].SetActive(true);
            SkinInfo skinInfo = player.selectedCharacter.GetComponent<SkinInfo>();
            if (skinInfo != null)
            {
                Image playerPic = playerScoreObjects[i].GetComponent<Image>();
                playerPic.sprite = skinInfo.GetSprite();
                TextMeshProUGUI playerScore = playerScoreObjects[i].GetComponentInChildren<TextMeshProUGUI>();
                playerScore.text = player.score.ToString();
            }
            i++;
        }
    }

    public void StartLevel(int levelNumber)
    {
        // Assumes that all levels / maps are in order starting at firstMapSceneNumber 
        SceneManager.LoadScene(firstMapSceneNumber + levelNumber);
    }

    public void StartOrangeArena()
    {
        StartLevel(0);
    }

    public void StartSecondMap()
    {
        StartLevel(1);
    }

    public void StartThirdMap()
    {
        StartLevel(2);
    }

    public void StartFourthMap()
    {
        StartLevel(3);
    }

    public void StartFifthMap()
    {
        StartLevel(4);
    }

    public void StartSixthMap()
    {
        StartLevel(5);
    }

    public void StartSeventhMap()
    {
        StartLevel(6);
    }

    public void MainMenu()
    {
        // Need to remove all "do not destroy items" AKA the players

        foreach (Player onePlayer in PersistentValues.persistentValues.players)
        {
            Destroy(onePlayer.playerInput.gameObject);
        }
        SceneManager.LoadScene(0);
    }
}
