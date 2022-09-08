using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string level;
    
    // these functions are connected to the respective main menu buttons
    public void Play()
    {
        SceneManager.LoadScene(level);
    }

    public void LevelSelect()
    {
        // TODO: switch scene to level select
        SceneManager.LoadScene("LevelSelect");
        Debug.Log("level select!");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("quitting!");
    }
}
