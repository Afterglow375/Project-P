using Managers;
using UI.Shared;
using UnityEngine;
using Utilities;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private SettingsManager _settingsManager;
    
        // these functions are connected to the respective main menu buttons
        public void Play()
        {
            SceneLoader.Instance.LoadScene(Scenes.Playground);
        }

        public void Overworld()
        {
            SceneLoader.Instance.LoadScene(Scenes.Overworld);
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
