using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI
{
    public class LevelFailed : MonoBehaviour
    {
        private GameObject _levelFailed;

        private void Awake()
        {
            CombatManager.LevelFailedEvent += OnLevelFailed;
        }

        private void OnDestroy()
        {
            CombatManager.LevelFailedEvent -= OnLevelFailed;
        }

        private void Start()
        {
            _levelFailed = transform.GetChild(0).gameObject;
            _levelFailed.SetActive(false);
        }
        
        private void OnLevelFailed()
        {
            _levelFailed.SetActive(true);
        }

        public void RetryLevel()
        {
            GameManager.Instance.ChangeScene(SceneManager.GetActiveScene().name);
        }

        public void MainMenu()
        {
            GameManager.Instance.ChangeScene(Scenes.MainMenu);
        }
    }
}