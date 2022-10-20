using Managers;
using UI.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI.BallArena
{
    public class LevelFailedUI : MonoBehaviour
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
            SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void MainMenu()
        {
            SceneLoader.Instance.LoadScene(Scenes.MainMenu);
        }
    }
}
