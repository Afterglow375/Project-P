using Managers;
using UI.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI.BallArena
{
    public class GameVictoryUI : MonoBehaviour
    {
        private GameObject _gameVictory;

        private void Awake()
        {
            CombatManager.GameVictoryEvent += OnGameVictory;
        }

        private void OnDestroy()
        {
            CombatManager.GameVictoryEvent -= OnGameVictory;
        }

        private void Start()
        {
            _gameVictory = transform.GetChild(0).gameObject;
            _gameVictory.SetActive(false);
        }
        
        private void OnGameVictory()
        {
            _gameVictory.SetActive(true);
            GameManager.Instance.LevelCompleted(SceneManager.GetActiveScene().name);
        }

        public void MainMenu()
        {
            SceneLoader.Instance.LoadScene(Scenes.MainMenu);
        }
    }
}