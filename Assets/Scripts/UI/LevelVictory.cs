using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI
{
    public class LevelVictory : MonoBehaviour
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
        }

        public void RetryLevel()
        {
            GameManager.Instance.ChangeScene(SceneManager.GetActiveScene().name);
        }

        // TODO: add level metadata so this function isn't terrible
        public void NextLevel()
        {
            int currLvl = SceneManager.GetActiveScene().name[-1] - '0';
            GameManager.Instance.ChangeScene("Level " + currLvl);
        }

        public void MainMenu()
        {
            GameManager.Instance.ChangeScene(Scenes.MainMenu);
        }
    }
}
