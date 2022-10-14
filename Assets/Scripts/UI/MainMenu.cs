using UnityEngine;
using Utilities;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private SettingsManager _settingsManager;
    
        // these functions are connected to the respective main menu buttons
        public void Play()
        {
            _sceneLoader.LoadScene(Scenes.Playground);
        }

        public void LevelSelect()
        {
            _sceneLoader.LoadScene(Scenes.LevelSelect);
        }

        public void Settings()
        {
            _settingsManager.ToggleSettingsMenuUI();
        }

        public void Quit()
        {
            Debug.Log("Quitting!");
            Application.Quit();
        }
    }
}
