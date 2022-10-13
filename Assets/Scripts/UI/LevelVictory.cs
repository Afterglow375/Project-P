using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI
{
    public class LevelVictory : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
        
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
            _sceneLoader.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LevelSelect()
        {
            _sceneLoader.LoadScene(Scenes.LevelSelect);
        }

        public void MainMenu()
        {
            _sceneLoader.LoadScene(Scenes.MainMenu);
        }
    }
}
