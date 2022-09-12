using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public string level;
    
        // these functions are connected to the respective main menu buttons
        public void Play()
        {
            GameManager.Instance.ChangeScene(level);
        }

        public void LevelSelect()
        {
            GameManager.Instance.ChangeScene(Scenes.LevelSelect);
        }

        public void Quit()
        {
            Debug.Log("Quitting!");
            Application.Quit();
        }
    }
}
