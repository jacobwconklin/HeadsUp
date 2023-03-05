
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private int freeForAllSceneNumber = 1;
    [SerializeField] private int teamTrialSceneNumber = 10;
    // [SerializeField] private int lastPairStandingSceneNumber = 2;
    // [SerializeField] private int campaignSceneNumber = 3;
    public void StartGame ()
    {

        /* Peak maintains some data this way not sure If I will in ChickenOff
        // Reset the Persistent Values, should really store them in a file but for now they are not. Resets only what is necessary
        PersistentValues.Initialize();
        PersistentValues.persistentValues.Reset();
        */

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Shuts down the whole game
    public void quitGame()
    {
        Application.Quit();
    }


    // Want this to be the following:
    // may also set up endless waves like gang beasts to coop and see how many kills you can
    // acheive with respawning teammates each wave and special boss on waves divisible by 20 and final boss at 100
    public void StartTeamTrial()
    {
        // Should load Character selection screen for FreeForAll game
        SceneManager.LoadScene(teamTrialSceneNumber);
    }

    public void StartFreeForAll()
    {
        // Should load Character selection screen for FreeForAll game
        SceneManager.LoadScene(freeForAllSceneNumber);
    }
    /*
    

    public void StartLastPairStanding()
    {
        // For now nothing but will be -> SceneManager.LoadScene(lastPairStandingSceneNumber);
    }

    public void StartCampaign()
    {
        // For now nothing but will be -> SceneManager.LoadScene(campaignSceneNumber);
    }
    */
}
