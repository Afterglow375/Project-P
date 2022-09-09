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
        Debug.Log("level select!");
        SceneManager.LoadScene("LevelSelect");
    }

    public void Quit()
    {
        Debug.Log("quitting!");
        Application.Quit();
    }
}
