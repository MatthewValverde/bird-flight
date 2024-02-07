using UnityEngine;
using UnityEngine.SceneManagement;

public class PathTest : MonoBehaviour
{
    public BirdController birdController;
    string[] triggers = { "FirstFlight", "SecondFlight" };
    int counter = 0;

    public void Advance()
    {
        if (counter == triggers.Length)
        {
            RestartScene();
            return;
        }
        birdController.SetTrigger(triggers[counter]);
        counter++;
    }

    void RestartScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}