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
            SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }

        // TODO: make this not terrible
        public void NextLevel()
        {
            string currLevel = SceneManager.GetActiveScene().name;
            string nextLevel = "";
            if (currLevel == Scenes.Level1)
            {
                nextLevel = Scenes.Level2;
            }
            else if (currLevel == Scenes.Level2)
            {
                nextLevel = Scenes.Level3;
            }
            else if (currLevel == Scenes.Level3)
            {
                nextLevel = Scenes.Level4;
            }
            else if (currLevel == Scenes.Level4)
            {
                nextLevel = Scenes.Level5;
            }
            else if (currLevel == Scenes.Level5)
            {
                nextLevel = Scenes.Level6;
            }

            SceneLoader.Instance.LoadScene(nextLevel);
        }

        public void MainMenu()
        {
            SceneLoader.Instance.LoadScene(Scenes.MainMenu);
        }
    }
}
