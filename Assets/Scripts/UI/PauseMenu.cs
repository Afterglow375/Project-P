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
                if (GameManager.Instance.State == GameState.Pause)
                {
                    ResumeGame();
                }
                else
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
            GameManager.Instance.State = _previousState;
        }

        public void PauseGame()
        {
            Debug.Log("Paused");
            _previousState = GameManager.Instance.State;
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
