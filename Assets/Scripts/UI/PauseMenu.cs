using Managers;
using UnityEngine;
using Utilities;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        private GameState _previousState;
        private GameObject _pauseMenuUI;
        
        private void Start()
        {
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
            GameManager.Instance.ChangeScene(Scenes.MainMenu);
        }
    }
}
