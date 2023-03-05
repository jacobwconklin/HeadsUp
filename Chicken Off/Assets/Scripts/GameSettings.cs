using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    // Holds the number for the GamesToWin selected by the players:
    [SerializeField] private TextMeshProUGUI gamesToWinText;

    // When players adjust the number of games they want on the slider, 
    // this function will update that number in the UI and in Persistent Values
    public void receiveNewNumberGamesToWin(System.Single gamesToWin)
    {
        PersistentValues.persistentValues.numGamesToWin = gamesToWin; 
        if (gamesToWin == 0)
        {
            gamesToWinText.text = "NO LIMIT";
        } 
        else
        {
            gamesToWinText.text = gamesToWin.ToString();
        }
    }

    public void begin()
    {
        // Load next scene (Level selector) To begin game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void back()
    {
        // Simply disable gameSettingsUI and allow players to continue choosing their characters
        PersistentValues.persistentValues.canSelectCharacters = true;
        this.gameObject.SetActive(false);
    }
    
}
