using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * Buttons Class
 *
 * Handles Start Screen Buttons
 * 
 * `LoadGame` - Loads the Main Scene
 * `ExitGame` - Exits the game
 */
public class Buttons : MonoBehaviour
{
    private const string MainSceneName = "MainScene";

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadGame()
    {
        ChangeScene(MainSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
