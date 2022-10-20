using Managers;
using UI.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI.BallArena
{
    public class LevelVictoryUI : MonoBehaviour
    {
        private GameObject _levelVictory;

        private void Awake()
        {
            CombatManager.LevelVictoryEvent += OnLevelVictory;
        }

        private void OnDestroy()
        {
            CombatManager.LevelVictoryEvent -= OnLevelVictory;
        }

        private void Start()
        {
            _levelVictory = transform.GetChild(0).gameObject;
            _levelVictory.SetActive(false);
        }
        
        private void OnLevelVictory()
        {
            _levelVictory.SetActive(true);
            GameManager.Instance.LevelCompleted(SceneManager.GetActiveScene().name);
        }

        public void RetryLevel()
        {
            SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Overworld()
        {
            SceneLoader.Instance.LoadScene(Scenes.Overworld);
        }

        public void MainMenu()
        {
            SceneLoader.Instance.LoadScene(Scenes.MainMenu);
        }
    }
}
