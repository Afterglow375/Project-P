using Managers;
using UnityEngine;
using Utilities;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        private GameState _previousState;
        private GameObject _pauseMenuUI;

        [SerializeField]
        private SceneLoader _sceneLoader;
        [SerializeField]
        private GameObject _settingsMenuParent;
        private SettingsManager _settingsManager;

        private void Start()
        {
            _settingsManager = _settingsMenuParent.GetComponent<SettingsManager>();
            _pauseMenuUI = transform.GetChild(0).gameObject;
            _pauseMenuUI.SetActive(false);
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameManager.Instance.state == GameState.Pause)
                {
                    ResumeGame();
                }
                else if (GameManager.Instance.state != GameState.LevelVictory || GameManager.Instance.state != GameState.LevelFailed)
                {
                    PauseGame();
                }
            }
        }

        public void ResumeGame()
        {
            Debug.Log("Resume: " + _previousState);
            Time.timeScale = 1f;
            _pauseMenuUI.SetActive(false);
            _settingsManager.CloseSettingsMenuUI();
            GameManager.Instance.state = _previousState;
        }

        public void PauseGame()
        {
            Debug.Log("Paused");
            _previousState = GameManager.Instance.state;
            GameManager.Instance.UpdateGameState(GameState.Pause);
            Time.timeScale = 0f;
            _pauseMenuUI.SetActive(true);
        }

        public void MainMenu()
        {
            Time.timeScale = 1f;
            _sceneLoader.LoadScene(Scenes.MainMenu);
        }

        public void SettingsMenu()
        {
            TogglePauseMenuUI();
            _settingsManager.ToggleSettingsMenuUI();
        }

        public void TogglePauseMenuUI()
        {
            _pauseMenuUI.SetActive(!_pauseMenuUI.activeSelf);
        }
    }
}
