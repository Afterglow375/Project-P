using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
    
        // these functions are connected to the respective main menu buttons
        public void Play()
        {
            _sceneLoader.LoadScene(LevelsHelper.GetFurthestLevel());
        }

        public void LevelSelect()
        {
            _sceneLoader.LoadScene(Scenes.LevelSelect);
        }

        public void Quit()
        {
            Debug.Log("Quitting!");
            Application.Quit();
        }
    }
}
