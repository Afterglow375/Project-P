using Managers;
using UnityEngine;
using Utilities;

namespace UI
{
    public class GameVictory : MonoBehaviour
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
        }

        public void MainMenu()
        {
            GameManager.Instance.ChangeScene(Scenes.MainMenu);
        }
    }
}